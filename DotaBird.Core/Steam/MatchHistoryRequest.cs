using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotaBird.Core.Steam
{
    /// <summary>
    /// Represents a request to search the match history for DOTA 2.
    /// </summary>
    public class MatchHistoryRequest
    {
        /// <summary>
        /// If set, restricts the results to games in which the named player was in.
        /// </summary>
        public String PlayerName { get; set; }

        /// <summary>
        /// If set, restricts the results to games equal to or older than this match id.
        /// </summary>
        public long? StartAtMatchId { get; set; }

        /// <summary>
        /// If set, restricts the results to games after this date/time.
        /// </summary>
        public DateTime? MinDate { get; set; }

        /// <summary>
        /// If set, restricts the results to games before this date/time.
        /// </summary>
        public DateTime? MaxDate { get; set; }
    }
}
