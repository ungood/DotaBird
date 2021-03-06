﻿using System;
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

    public class MatchHistory
    {
        public long GetLastMatchId()
        {
            // There was a pretty serious bug here.
            // You are not guaranteed this will always have 25 items in it.
            // And in fact, it was returning 100 results at a time.
            if (Matches.Count == 0)
                return -1;
            return Matches[Matches.Count - 1].Id;
        }

        [JsonProperty("num_results")]
        public int NumResults { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        [JsonProperty("results_remaining")]
        public int ResultsRemaining { get; set; }

        [JsonProperty("matches")]
        public List<MatchSummary> Matches { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class MatchSummary
    {
        [JsonProperty("match_id")]
        public long Id { get; set; }

        [JsonProperty("match_seq_num")]
        public long SeqNum { get; set; }

        [JsonProperty("start_time")]
        [JsonConverter(typeof(SteamDateTimeConverter))]
        public DateTime StartTime { get; set; }

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
