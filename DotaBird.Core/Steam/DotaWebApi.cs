using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace DotaBird.Core.Steam
{
    public class DotaWebApi : IDotaWebApi
    {
        public MatchHistory GetMatchHistory(MatchHistoryRequest request)
        {
            var client = new WebClient();
            string requestString = "";

            if (request.PlayerName != null)
                requestString = "player_name=" + request.PlayerName + '&';
            if (request.StartAtMatchId != null)
                requestString = requestString + "start_at_match_id=" + request.StartAtMatchId + '&';
            if (request.MinDate != null)
                requestString = requestString + "date_min=" + request.MinDate + '&';
            if (request.MaxDate != null)
                requestString = requestString + "date_max=" + request.MaxDate + '&';

            string url = "https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?" + requestString + "key=A41B14673A53F4C0A5281A6C47637C9E";

            string json = client.DownloadString(url);

            MatchHistory matchHistory = JsonConvert.DeserializeObject<MatchHistory>(json);

            return matchHistory;
        }
    }
}
