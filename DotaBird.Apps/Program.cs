using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DotaBird.Core.Steam;

namespace DotaBird.Apps
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotaWebApi api = new DotaWebApi();
            MatchHistoryRequest request = new MatchHistoryRequest();
            request.PlayerName = "wovoka";
            MatchHistory matchHistory = api.GetMatchHistory(request);
            Console.WriteLine("Match History: " + matchHistory);
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
