using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization.Json;

namespace Perceptive.ARR.HelperLibrary
{
    public class RestServiceUtility
    {
        private static string RepositoryManagerEndpoint
        {
            get { return ConfigurationManager.AppSettings["RepositoryManagerEndpoint"]; }
        }

        public static string GetLogsUrl()
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/GetLogs", RepositoryManagerEndpoint);
        }

        public static string GetUsersUrl()
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/GetUsers", RepositoryManagerEndpoint);
        }

        public static string ModifyUserUrl()
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/ModifyUser", RepositoryManagerEndpoint);
        }

        public static string GetSchedulerUrl()
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/GetScheduler", RepositoryManagerEndpoint);
        }

        public static string SetSchedulerUrl()
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/SetScheduler", RepositoryManagerEndpoint);
        }

        public static string AuthenticateUserUrl()
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/AuthenticateUser", RepositoryManagerEndpoint);
        }

        public static string GetSupportedLogTypesUrl()
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/GetSupportedLogTypes", RepositoryManagerEndpoint);
        }

        public static string DeleteUserUrl(string userId)
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/DeleteUser/{1}", RepositoryManagerEndpoint, userId);
        }

        public static string GetSpecificLogUrl(Guid logId, Guid userId)
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/{1}/UserId/{2}", GetLogsUrl(), logId.ToString(), userId.ToString());
        }

        public static string StartSchedulerUrl(bool start)
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/Scheduler/{1}", RepositoryManagerEndpoint, (start ? 1 : 0));
        }        

        public static string ProcessLogsUrl(string task, int days)
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/{1}/{2}", RepositoryManagerEndpoint, task, days);
        }

        public static string AddSupportedLogTypeUrl(string code, string displayName)
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/AddSupportedLogType/Code/{1}/DisplayName/{2}", RepositoryManagerEndpoint, code, displayName);
        }

        public static string DeleteSupportedLogTypeUrl(string code, string displayName)
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/DeleteSupportedLogType/Code/{1}/DisplayName/{2}", RepositoryManagerEndpoint, code, displayName);
        }

        public static string GetAppSettingUrl()
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/GetAppSetting", RepositoryManagerEndpoint);
        }

        public static string SetAppSettingUrl()
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/SetAppSetting", RepositoryManagerEndpoint);
        }

        public static string GetDatabaseListUrl(string userId)
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/Databases/{1}", RepositoryManagerEndpoint, userId);
        }

        public static string SetDatabaseListUrl(string userId, string databaseName)
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}/Databases/{1}/ActiveDatabase/{2}", RepositoryManagerEndpoint, userId, databaseName);
        }

        public static string GetJsonString(object obj)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            List<Type> knownTypes = new List<Type>();
            knownTypes.Add(typeof(AppSettingItem));
            knownTypes.Add(typeof(List<AppSettingItem>));

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(object), knownTypes);
            serializer.WriteObject(ms, obj);
            ms.Position = 0;
            System.IO.StreamReader sr = new System.IO.StreamReader(ms);
            return sr.ReadToEnd();
        }

        public static T GetObjectFromJson<T>(string jsonObject)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            System.IO.MemoryStream ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(jsonObject));
            return (T)serializer.ReadObject(ms);
        }

        private static HttpWebRequest GetRequest(WebRequestInput input)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(input.Uri);

            var isProxy = ConfigurationManager.AppSettings["ProxyAddress"];
            if (!string.IsNullOrEmpty(isProxy))
                request.Proxy = new WebProxy(new Uri(isProxy), true);

            request.Timeout = int.Parse(ConfigurationManager.AppSettings["RequestTimeout"]);
            request.Method = input.Method;
            return request;
        }

        public T ProcessRequest<T>(WebRequestInput input)
        {
            var request = GetRequest(input);

            if (request.Method.Equals(Constants.MethodPost))
            {
                request.ContentType = "application/json";
                byte[] postData = Encoding.UTF8.GetBytes(GetJsonString(input.RequestData));
                request.ContentLength = postData.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postData, 0, postData.Length);
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if(response.StatusCode == HttpStatusCode.OK)
            {
                DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(T));                
                var respObj = obj.ReadObject(response.GetResponseStream());
                return (T)respObj;
            }
            return default(T);
        }
    }

    public class WebRequestInput
    {
        public string Uri { get; set; }
        public string Method { get; set; }
        public object RequestData { get; set; }
    }
}
