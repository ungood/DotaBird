using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DotaBird.Core.Steam;
using DotaBird.Core.Net;

namespace DotaBird.Apps
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebClient webClient = new WebClient();
            DotaWebApi api = new DotaWebApi(webClient);
            MatchHistoryRequest request = new MatchHistoryRequest();
            request.PlayerName = "wovoka";
            MatchHistory matchHistory = api.GetMatchHistory(request);
            Console.WriteLine(matchHistory);
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
