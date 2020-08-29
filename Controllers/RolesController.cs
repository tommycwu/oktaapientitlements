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
    public class RolesController : ApiController
    {
        public class UserRole
        {
            public string id, role, profile, title;
        };
        public class AppId
        {
            public string name, id;
        }

        public List<string> GetUsersList(string apiUrl, string apikey)
        {
            List<string> responseList = new List<string>(new string[] { "" });
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
                        for (int i = 0; i <= jsonObject.Count - 1; i++)
                        {
                            responseList.Add(jsonObject[i].id.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseList.Add(ex.Message);
                return responseList;
            }
            return responseList;
        }

        public dynamic GetRoleProfile(string apiUrl, string apikey)
        {
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
                            tempObj.role = jsonObject.profile.role;
                            tempObj.profile = jsonObject.profile.profile;
                            tempObj.title = jsonObject.profile.title;
                            return tempObj;
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        [Route("api/v1/roles/{appId}")]
        [HttpGet]
        // GET api/<controller>/5
        public string Get(string appId)
        {
            var re = Request;
            var headers = re.Headers;
            string apiKey = "";
            string OktaOrg = "";
            string retString = "";
            UserRole retObj = new UserRole();
            List<string> usersList = new List<string>();
            List<string> stringList = new List<string>();
            List<UserRole> objectList = new List<UserRole>();
            string rolesResult = "";

            if (headers.Contains("OktaOrg") && headers.Contains("Authorization"))
            {
                OktaOrg = headers.GetValues("OktaOrg").First();
                apiKey = headers.GetValues("Authorization").First();
                usersList = GetUsersList(OktaOrg + "/api/v1/apps/" + appId + "/users", apiKey);
                if (usersList[0].ToString() == "Error...")
                {
                    return usersList[1].ToString();
                }
                else
                {
                    for (int i = 0; i <= usersList.Count - 1; i++)
                    {
                        string buildUrl = OktaOrg + "/api/v1/apps/" + appId + "/users/" + usersList[i];
                        retObj = GetRoleProfile(buildUrl, apiKey);
                        if (retObj != null)
                        { 
                            retString = JsonConvert.SerializeObject(retObj, Formatting.None);
                            if (!stringList.Contains(retString))
                            {
                                stringList.Add(retString);
                                objectList.Add(retObj);
                            }
                        }
                    }
                    rolesResult = JsonConvert.SerializeObject(objectList, Formatting.None);
                }
            }
            else
            {
                rolesResult = "ERROR - Missing OktaOrg or Authorization Header";
            }
            return rolesResult;
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