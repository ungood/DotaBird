using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DotaBird.Core.Twitter
{
    /// Gets the list of players requested with attached twitter handles from an outside website? another class?  Use a hashmap or dictionary? 
    /// (playerID, twitter handle)
    public class PlayerRequests
    {
        private Dictionary<long, string> requestDictionary = new Dictionary<long, string>();
        
        public Dictionary<long, string> GetRequests()
        {
            return requestDictionary;
        }

        public void AddRequest(long playerRequested, string requestor)
        {
            requestDictionary.Add(playerRequested, requestor);
        }
    }
}
