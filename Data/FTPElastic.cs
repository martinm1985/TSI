using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Data
{
    public class FTPElastic
    {

        public static string UploadFileFTP(byte[] fileContents, string extension)
        {
            // Get the object used to communicate with the server.
            string url = "ftp://jelastic-ftp:8Vll14gkft@179.31.2.37/Files/";

            string id = System.Guid.NewGuid().ToString();
            url += id + "." + extension;

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential("jelastic-ftp", "8Vll14gkft");

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return id; //($"Upload File Complete, status {response.StatusDescription}");
            }

        }

        public static byte[] DownloadFileFTP(string filename)
        {
            string url = "ftp://jelastic-ftp:8Vll14gkft@179.31.2.37/Files/";
            url += filename;

            try
            {
                /* Create an FTP Request */
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(url);
                request.Credentials = new NetworkCredential("jelastic-ftp", "8Vll14gkft");
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = true;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                
                Stream responseStream = response.GetResponseStream();
                using var ms = new MemoryStream();
                responseStream.CopyTo(ms); 
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
    }
}
