using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DotaBird.Core.Net
{
    public class WebClient : IWebClient
    {
        public string Get(Uri uri)
        {
            WebRequest request = WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }
    }
}
