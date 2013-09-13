using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using NLog;

using DotaBird.Core.Net;

namespace DotaBird.Core.Steam
{
    public class DotaWebApi : IDotaWebApi
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IWebClient webClient;
        
        public DotaWebApi(IWebClient webClient) {
            this.webClient = webClient;
        }
        
        public MatchHistory GetMatchHistory(MatchHistoryRequest request)
        {
            logger.Debug("Calling GetMatchHistory with {0}", request);
            string url = "https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key=A41B14673A53F4C0A5281A6C47637C9E";

            Uri uri = new Uri(url)
                .AddQuery("player_name", request.PlayerName)
                .AddQuery("start_at_match_id", request.StartAtMatchId)
                .AddQuery("date_min", request.MinDate)
                .AddQuery("date_max", request.MaxDate);
            
            string json = webClient.Get(uri);

            MatchHistoryEnvelope envelope = JsonConvert.DeserializeObject<MatchHistoryEnvelope>(json);
            MatchHistory history = envelope.Result;
            logger.Debug("Got {0} results starting at {1}; Remaining: {2}; Total:{3}",
                history.NumResults,
                history.Matches.Count == 0 ? "N/A" : history.Matches[0].Id.ToString(),
                history.ResultsRemaining,
                history.TotalResults);
            return history;
        }

        public MatchHistory GetMatchHistory()
        {
            return GetMatchHistory(new MatchHistoryRequest());
        }
    }
}
