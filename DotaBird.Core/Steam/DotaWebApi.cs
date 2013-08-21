using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;

using DotaBird.Core.Net;

namespace DotaBird.Core.Steam
{
    public class DotaWebApi : IDotaWebApi
    {
        private readonly IWebClient webClient;
        
        public DotaWebApi(IWebClient webClient) {
            this.webClient = webClient;
        }
        
        public MatchHistory GetMatchHistory(MatchHistoryRequest request)
        {
            string url = "https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key=A41B14673A53F4C0A5281A6C47637C9E";

            Uri uri = new Uri(url)
                .AddQuery("player_name", request.PlayerName)
                .AddQuery("start_at_match_id", request.StartAtMatchId)
                .AddQuery("date_min", request.MinDate)
                .AddQuery("date_max", request.MaxDate);


            string json = webClient.Get(uri);

            MatchHistoryEnvelope matchHistory = JsonConvert.DeserializeObject<MatchHistoryEnvelope>(json);


            return matchHistory.Result;
        }
    }
}
