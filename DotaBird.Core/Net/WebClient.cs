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

        private System.Net.WebClient client = new System.Net.WebClient();

        public string Get(Uri uri)
        {
            return client.DownloadString(uri);
        }
    }
}
