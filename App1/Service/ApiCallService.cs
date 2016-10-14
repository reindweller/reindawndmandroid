using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace App1.Service
{
    public static class ApiCallService
    {

        public static HttpWebResponse CreateRequest<TModel>(TModel model, string url)
        {
            var data = JsonConvert.SerializeObject(model);
            var address = new Uri(url);
            var byteData = UTF8Encoding.UTF8.GetBytes(data);
            var request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json; encoding='utf-8'";
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response;
        }

        //public static async Task<HttpWebResponse> CreateRequest<TModel>(TModel model, string url)
        //{

        //    var data = JsonConvert.SerializeObject(model);
        //    var address = new Uri(url);
        //    var byteData = UTF8Encoding.UTF8.GetBytes(data);
        //    var request = WebRequest.Create(address) as HttpWebRequest;
        //    request.Method = "POST";
        //    request.ContentType = "application/json; encoding='utf-8'";
        //    request.ContentLength = byteData.Length;
        //    using (Stream postStream = await request.GetRequestStreamAsync())
        //    {
        //        postStream.Write(byteData, 0, byteData.Length);
        //    }

        //    HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        //    return response;
        //}

        public static HttpWebResponse GetAllRequest(string url)
        {
            var address = new Uri(url);
            var request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json; encoding='utf-8'";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response;
        }

        //public string PostRequest<TModel>(TModel model, string endpoint)
        //{
        //    var data = JsonConvert.SerializeObject(model);
        //    var address = new Uri(endpoint);
        //    var byteData = UTF8Encoding.UTF8.GetBytes(data);
        //    //var key = Md5Utils.CreateKeyMd5(string.Format("{0}{1:yyyyMMdd}{2}", IpAddress, DateTime.Now, AuthToken));
        //    var request = WebRequest.Create(address) as HttpWebRequest;
        //    request.Method = "POST";
        //    request.ContentType = "application/json; encoding='utf-8'";
        //    request.ContentLength = byteData.Length;
        //    //request.Headers.Add("CustomerNo", CustomerNumber.ToString());
        //    //request.Headers.Add("Key", key);
        //    using (Stream postStream = request.GetRequestStream())
        //    {
        //        postStream.Write(byteData, 0, byteData.Length);
        //    }

        //    try
        //    {
        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            using (var reader = new StreamReader(response.GetResponseStream()))
        //            {
        //                Stream responseStream = response.GetResponseStream();
        //                string responseStr = new StreamReader(responseStream).ReadToEnd();
        //                return responseStr;
        //                //return XmlUtils.Deserialize<TResponse>(reader);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return string.Empty;
        //    }


        //    //return string.Empty;
        //}


        //public string GetAllRequest(string endpoint)
        //{
        //    //var data = JsonConvert.SerializeObject(model);
        //    var address = new Uri(endpoint);
        //    //var byteData = UTF8Encoding.UTF8.GetBytes(data);
        //    //var key = Md5Utils.CreateKeyMd5(string.Format("{0}{1:yyyyMMdd}{2}", IpAddress, DateTime.Now, AuthToken));
        //    var request = WebRequest.Create(address) as HttpWebRequest;
        //    request.Method = "GET";
        //    request.ContentType = "application/json; encoding='utf-8'";
        //    //request.ContentLength = byteData.Length;

        //    //using (Stream postStream = request.GetRequestStream())
        //    //{
        //    //    postStream.Write(byteData, 0, byteData.Length);
        //    //}

        //    try
        //    {
        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            using (var reader = new StreamReader(response.GetResponseStream()))
        //            {
        //                Stream responseStream = response.GetResponseStream();
        //                string responseStr = new StreamReader(responseStream).ReadToEnd();
        //                return responseStr;
        //                //return XmlUtils.Deserialize<TResponse>(reader);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return string.Empty;
        //    }


        //    //return string.Empty;
        //}
    }
}