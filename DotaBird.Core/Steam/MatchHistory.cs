using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DotaBird.Core.Steam
{
    /// <summary>
    /// If you look carefully, you'll notice the JSON returned by GetMatchHistory is not, in fact, a MatchHistory object,
    /// it is an object with a single property, "result", which is the MatchHistory data.
    /// </summary>
    public class MatchHistoryEnvelope
    {
        public MatchHistory Result { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MatchHistory
    {
        [JsonProperty("num_results")]
        public int NumResults { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        [JsonProperty("results_remaining")]
        public int ResultsRemaining { get; set; }

        [JsonProperty("matches")]
        public List<MatchSummary> Matches { get; set; }
    }

    public class MatchSummary
    {
        [JsonProperty("match_id")]
        public long MatchId { get; set; }

        [JsonProperty("match_seq_num")]
        public long MatchSeqNum { get; set; }

        [JsonProperty("start_time")]
        public long StartTime { get; set; }

        [JsonProperty("lobby_type")]
        public int LobbyType { get; set; }

        [JsonProperty("players")]
        public List<PlayerSummary> Players { get; set; }
    }

    public class PlayerSummary
    {
        [JsonProperty("account_id")]
        public long AccountId { get; set; }

        [JsonProperty("player_slot")]
        public int PlayerSlot { get; set; }

        [JsonProperty("hero_id")]
        public int HeroId { get; set; }
    }
}
