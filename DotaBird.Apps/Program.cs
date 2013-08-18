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
            String matchHistory = api.GetMatchHistory();
            Console.WriteLine("Match History: " + matchHistory);
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
