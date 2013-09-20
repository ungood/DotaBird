using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotaBird.Core.Twitter
{
    /// Compares the list that GetRequests gets to the matches polled from the api
    /// If there is a match and if the requestor is allowed to access that match summary
    /// call Twitter Handler to handle the twitter posting 
    public class CheckPlayerRequested
    {
        private string ConvertPlayerIDToName(long steamId)
        {
            string playerName ="";

            return playerName;
        }

        private bool IsPlayerRequestedAccessable(long steamId)
        {

            return true;
        }

        private void CheckForMatch()
        {

            //new TwitterHandler().PostOnTwitter(summary, requestor, playerRequested);
        }
        

    }
}
