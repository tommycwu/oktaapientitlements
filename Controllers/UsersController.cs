using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OktaEntitlements.Controllers
{
    public class UsersController : ApiController
    {
        public class UserName
        {
            public string name, id;
        }

        public string GetUsersList(string apiUrl, string apikey)
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
                        var userList = new List<UserName>();
                        for (int i = 0; i <= jsonObject.Count - 1; i++)
                        {
                            var tempObj = new UserName();
                            tempObj.id = jsonObject[i].id;
                            tempObj.name = jsonObject[i].credentials.userName;
                            userList.Add(tempObj);
                        }
                        retJson = JsonConvert.SerializeObject(userList, Formatting.None);
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
        // GET api/<controller>/5
        [Route("api/v1/users/{appId}")]
        [HttpGet]
        public string Get(string appId)
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
                string buildAppUrl = OktaOrg + "/api/v1/apps/" + appId + "/users";
                retResult = GetUsersList(buildAppUrl, apiKey);
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