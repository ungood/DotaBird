using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public int NumResults { get; set; }
        public int TotalResults { get; set; }
        public int ResultsRemaining { get; set; }

        public List<MatchSummary> Matches { get; set; }
    }

    public class MatchSummary
    {
        public long MatchId { get; set; }
    }
}
