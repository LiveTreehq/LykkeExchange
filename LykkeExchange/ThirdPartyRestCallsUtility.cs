using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ExchangeMarket.LykkeExchange
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
        public static string GET(string url, Dictionary<string, string> headers = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    string value;
                    headers.TryGetValue(key, out value);
                    request.Headers.Add(key, value);
                }
            }
            try
            {
                WebResponse response = request.GetResponse();
                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.NoContent)
                    return null;

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
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
        public static string POST(string url, string jsonContent, Dictionary<string, string> headers = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    string value;
                    headers.TryGetValue(key, out value);
                    request.Headers.Add(key, value);
                }
            }

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                    if (response.StatusCode == HttpStatusCode.NoContent)
                        return null;

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                // Log exception and throw as for GET example above
                return null;
            }
        }
    }
}
