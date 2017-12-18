using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace ChinaBeacon.Sdk.Core.Tools
{
    public static class RestClientHelper
    {

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <typeparam name="T">指定返回值的模型</typeparam>
        /// <param name="request"></param>
        /// <param name="baseUrl"></param>
        /// /// <param name="method"> get or post</param>
        /// <returns></returns>
        public static T Execute<T>(RestRequest request, string baseUrl,Method method = Method.POST) where T : new()
        {
            request.Method = method;
            var client = new RestClient { BaseUrl = new System.Uri(baseUrl) };
            //            client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
            //            request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment); // used on every request
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return response.Data;
        }
        public static void Execute(RestRequest request, string baseUrl, Method method = Method.POST)
        {
            request.Method = method;
            var client = new RestClient { BaseUrl = new System.Uri(baseUrl) };
           
            var response = client.Execute(request);
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
        }
        
        
    }

    public class AuthServerResponse
    {
        public string Message { get; set; }

        public bool IsSuccessed { get; set; }
        public object Result { get; set; }
    }

}
