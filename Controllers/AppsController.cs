using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace OktaEntitlements.Controllers
{
    public class AppsController : ApiController
    {
        public class AppId
        {
            public string name, id;
        }

        public string GetAllApps(string apiUrl, string apikey)
        {
            string retJson = "";
            try
            {
                var endpoint = new Uri(apiUrl);
                var webRequest = WebRequest.Create(endpoint) as HttpWebRequest;
                if (webRequest != null)

                {
                    webRequest.Method = "GET";
                    webRequest.Headers.Add("Authorization", "SSWS " + apikey);
                    webRequest.Accept = "application/json";
                    webRequest.ContentType = "application/json";
                    var response = webRequest.GetResponse();
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string jsonString = reader.ReadToEnd();
                        dynamic jsonObject = JsonConvert.DeserializeObject(jsonString, typeof(object));
                        var appList = new List<AppId>();
                        for (int i = 0; i <= jsonObject.Count - 1; i++)
                        {
                            var tempObj = new AppId();
                            tempObj.id = jsonObject[i].id;
                            tempObj.name = jsonObject[i].name;
                            appList.Add(tempObj);
                        }
                        retJson = JsonConvert.SerializeObject(appList, Formatting.None);
                    }
                }
                else
                {
                    retJson = "webRequest = null";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return retJson;
        }

        //GET api/<controller>

        [Route("api/v1/apps")]
        [HttpGet]
        public string Get()
        {
            var re = Request;
            var headers = re.Headers;
            string apiKey = "";
            string OktaOrg = "";
            string retResult = "";

            if (headers.Contains("OktaOrg") && headers.Contains("Authorization"))
            {
                OktaOrg = headers.GetValues("OktaOrg").First();
                apiKey = headers.GetValues("Authorization").First();
                retResult = GetAllApps(OktaOrg + "/api/v1/apps/", apiKey);
            }
            else
            {
                retResult = "ERROR - Missing OktaOrg or Authorization Header";
            }
            return retResult;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}