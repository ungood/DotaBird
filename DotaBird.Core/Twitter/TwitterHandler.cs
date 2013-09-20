using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using TweetSharp;

using DotaBird.Core.Steam;

namespace DotaBird.Core.Twitter
{
    public class TwitterHandler
    {
        /// call to twitter to write the player's match summary to the requestor's twitter handle 
        public void PostOnTwitter(MatchSummary summary, string requestor, string playerRequested)
        {
            TwitterService service = GetAuthService();

            SendTweetOptions options = new SendTweetOptions() 
            { Status = LoadUpStatusText(summary, requestor, playerRequested) };

            service.SendTweet(options);
        }
        
        private TwitterService GetAuthService()
        {
            string consumerKey = ConfigurationManager.AppSettings["consumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
            string accessKey = ConfigurationManager.AppSettings["accessKey"];
            string accessSecret = ConfigurationManager.AppSettings["accessSecret"];

            // Pass credentials to the service
            TwitterService service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(accessKey, accessSecret);

            return service;
        }

        private string LoadUpStatusText(MatchSummary summary, string requestor, string playerRequested)
        {
            const string url = "http://dotabuff.com/matches/";
            string twitterHandle = "@" + requestor + '\n';
            string playerName = playerRequested;

            return twitterHandle + playerName + " just finished his/her match! To find out more go to " + url + summary.Id;
        }
    }
}
