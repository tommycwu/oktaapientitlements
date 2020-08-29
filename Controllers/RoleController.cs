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
    public class RoleController : ApiController
    {
        public class UserRole
        {
            public string id, role, profile, title;
        };

        public string GetRoleProfile(string apiUrl, string apikey)
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
                        var roleList = new List<UserRole>();
                        try
                        {
                            var tempObj = new UserRole();
                            tempObj.id = jsonObject.id;
                            tempObj.role = jsonObject.profile.role;
                            tempObj.profile = jsonObject.profile.profile;
                            tempObj.title = jsonObject.profile.title;
                            roleList.Add(tempObj);
                            retJson = JsonConvert.SerializeObject(roleList, Formatting.None);
                        }
                        catch
                        {

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return retJson;
        }

        [Route("api/v1/role/{appId}/{userId}/")]
        [HttpGet]
        public string Get(string userId, string appId)
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
                string buildAppUrl = OktaOrg + "/api/v1/apps/" + appId + "/users/" + userId;
                retResult = GetRoleProfile(buildAppUrl, apiKey);
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