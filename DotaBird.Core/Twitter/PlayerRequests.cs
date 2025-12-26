using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace DotaBird.Core.Twitter
{
    public class PlayerRequests
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        [JsonProperty("lastTweetId")]
        public long LastTweetId { get; set; }

        [JsonProperty("requestors")]
        public List<Requestor> Requestors { get; set; }
    }

    public class Requestor
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("playerRequested")]
        public long PlayerRequested { get; set; }
    }

    
}
