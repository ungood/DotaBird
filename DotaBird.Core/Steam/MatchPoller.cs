using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace DotaBird.Core.Steam
{
    public class MatchPoller : IMatchPoller
    {
        private readonly IDotaWebApi api;
        private readonly int throttle;

        public MatchPoller(IDotaWebApi api, int throttle)
        {
            this.api = api;
            this.throttle = throttle;
        }

        public IEnumerable<MatchSummary> PollMatches()
        {
            if (!EventLog.SourceExists("MySource"))
                EventLog.CreateEventSource("MySource", "MyNewLog");

            List<long> ids = new List<long>();

            int count = 0;
            bool ranIntoADuplicate = false;
            const int ONE_MILLION = 1000000;
            
            while(true) 
            {
                foreach(MatchSummary summary in GetNextBatch()) 
                {
                    if (ids.Contains(summary.Id))
                    {
                        ranIntoADuplicate = true;
                        break;
                    }

                    ids.Add(summary.Id);

                    yield return summary;
                    
                    count++;

                    if (count == ONE_MILLION)
                    {
                        // 1 long = 8 bytes; 1 million longs = 8 MB.  I don't want to exceed that much ram used,
                        // so when that limit is reached, I clear the oldest/bottom half of the list.  
                        // This will probably never happen.
                        ids.RemoveRange(0, ONE_MILLION / 2);
                        count = ONE_MILLION / 2;
                    }
                }

                if (!ranIntoADuplicate)
                {
                    EventLog myLog = new EventLog();
                    myLog.Source = "MySource";
                    myLog.WriteEntry("Never ran into a duplicate.", EventLogEntryType.Warning);

                    Console.WriteLine("Warning: Never ran into a duplicate.");
                }

                Thread.Sleep(throttle);
            }
        }

        public IEnumerable<MatchSummary> GetNextBatch()
        {
            foreach (MatchHistory history in GetPages())
            {
                foreach (MatchSummary match in history.Matches)
                {
                    yield return match;
                }
            }
        }

        public IEnumerable<MatchHistory> GetPages() 
        {
            MatchHistory history = api.GetMatchHistory();
            yield return history;

            while(history.ResultsRemaining > 0) 
            {
                MatchHistoryRequest request = new MatchHistoryRequest()
                {
                    StartAtMatchId = history.GetLastMatchId() - 1
                };
                history = api.GetMatchHistory(request);
                yield return history;
            }
        }
    }
}
