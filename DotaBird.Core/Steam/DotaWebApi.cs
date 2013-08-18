using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
//using System.Web.Script.Serialization;

namespace DotaBird.Core.Steam
{
    public class DotaWebApi
    {
        public string GetMatchHistory()
        {
            var client = new WebClient();

            var json = client.DownloadString("https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key=A41B14673A53F4C0A5281A6C47637C9E");

            return json;
        }
    }
}
