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
        private const IWebClient webClient;
        
        public DotaWebApi(IWebClient webClient) {
            this.webClient = webClient;
        }
        
        public MatchHistory GetMatchHistory(MatchHistoryRequest request)
        {
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
            Uri uri = new Uri(url); // TODO: See the UriExtensions to add your query parameters more elegantly.

            string json = webClient.Get(uri);

            MatchHistoryEnvelope matchHistory = JsonConvert.DeserializeObject<MatchHistoryEnvelope>(json);


            return matchHistory.Result;
        }
    }
}
