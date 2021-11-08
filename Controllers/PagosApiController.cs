using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Crud.Models;
using Crud.Data;
using Microsoft.Extensions.Options;
using System.Net.Http;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;

namespace Crud.Controllers
{
    [ApiController]
    public class PagosApiController: ControllerBase
    {

        public class AmountWithBreakdown
        {
            public string currency_code { get; set; }
            public string value { get; set; }
        }

        public class PurchaseUnitRequest
        {
            public AmountWithBreakdown amount { get; internal set; }
        }

        public class OrderRequest
        {


            public string intent { get; set; }
            public List<PurchaseUnitRequest> purchase_units { get; set; }
            public object redirect_urls { get; set; }
        }

        internal class ApplicationContext
        {

            public string return_url { get; set; }
            public string cancel_url { get; set; }
        }

        public class ResponseCheckout
        {
            public string id { get; set; }
            public string status { get; set; }
            public List<Link> links { get; set; }
        }

        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }

        static HttpClient client = new HttpClient();

        private readonly ApplicationDbContext _context;

        private readonly IOptions<PaypalApiSetting> pSetting;

        public PagosApiController(ApplicationDbContext context, IOptions<PaypalApiSetting> setting)
        {
            this._context = context;

            this.pSetting = setting;

        }

        [HttpPost]
        [Route("api/pagos/Paypal")]
        public void PagoPorPaypal([FromBody] AmountWithBreakdown amountFromBody)
        {

            // Construct a request object and set desired parameters
            // Here, OrdersCreateRequest() creates a POST request to /v2/checkout/orders
            var order = new OrderRequest()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        amount = amountFromBody
                    }
                },
                redirect_urls = new ApplicationContext()
                {
                    return_url = "https://www.example.com",
                    cancel_url = "https://www.example.com"
                }
            };



            string username = this.pSetting.Value.ClientID;
            string password = this.pSetting.Value.Secret;
            string baseApi = this.pSetting.Value.PayPalApiBase;
            string urlCheckout = baseApi + "v2/checkout/orders";

            var client = new RestClient(urlCheckout);
            client.Authenticator = new HttpBasicAuthenticator(username, password);
            var request = new RestRequest();
            //Creating a JavaScriptSerializer Object
            //Use of JsonConvert.SerializeObject()
            string jsonString = JsonConvert.SerializeObject(order);

            request.AddJsonBody(jsonString);
            var response = client.Post(request);
            var content = response.Content; // Raw content as string

            string redirectUrl = "";

            foreach (Link item in JsonConvert.DeserializeObject<ResponseCheckout>(response.Content).links)
            {
                if (item.rel == "approve")
                {
                    redirectUrl = item.href;
                    break;
                }
            };

            Response.Redirect(redirectUrl);

        }

    }

}
