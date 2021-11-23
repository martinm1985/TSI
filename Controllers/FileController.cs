using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.Models;
using Crud.DTOs;
using Crud.Services;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Crud.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {

        public FileController()
        {
        }

        [HttpPost("upload")]
        public async Task<ActionResult<string>> FileUpload(FileDto.File file)
        {
            Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
            string data = regex.Replace(file.Data, string.Empty);
            byte[] fileContents = Convert.FromBase64String(data);
            var resUpdate = Data.FTPElastic.UploadFileFTP(fileContents, file.Extension);
            return resUpdate;
        }

    }
}

