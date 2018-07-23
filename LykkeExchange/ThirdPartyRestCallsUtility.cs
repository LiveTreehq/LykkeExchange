
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ExchangeMarket
{
    /// <summary>
    ///REST calls utilities 
    /// </summary>
    internal class ThirdPartyRestCallsUtility
    {

        /// <summary>
        ///  GET request.
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="headers">headers</param>
        /// <returns>
        /// returns response json string if the query is successful and returns null if the response and NO Content
        /// </returns>
        /// <exception cref="WebException"></exception>
        public static string Get(string url, Dictionary<string, string> headers = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    string value;
                    headers.TryGetValue(key, out value);
                    request.Headers.Add(key, value);
                }
            }
            try
            {
                var response = request.GetResponse();
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.NoContent)
                    return null;

                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                using (var responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    var errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        /// <summary>
        /// POST Request for Lykke MarketOrder
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="jsonContent">headers</param>
        /// <param name="headers"></param>
        /// <returns>
        /// returns response json string if the query is successful and returns null if the response and NO Content
        /// </returns>
        /// <exception cref="WebException"></exception>
        public static string Post(string url, string jsonContent, Dictionary<string, string> headers = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    string value;
                    headers.TryGetValue(key, out value);
                    request.Headers.Add(key, value);
                }
            }

            if (jsonContent != null && jsonContent.Length > 0)
            {
                var byteArray = Encoding.UTF8.GetBytes(jsonContent);
                request.ContentLength = byteArray.Length;
                request.ContentType = @"application/json";

                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
            }

            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                    if (response.StatusCode == HttpStatusCode.NoContent)
                        return null;

                    using (var responseStream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                // Log exception and throw as for GET example above
                return null;
            }
        }
    }
}
