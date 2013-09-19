using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;

using System.Configuration;
using TweetSharp;

namespace DotaBird.Core.Twitter
{
    public class TwitterHandler
    {
        /// call to twitter to write the player's match summary to the requestor's twitter handle 
        public void PostOnTwitter()
        {
            TwitterService service = GetAuthService();
            SendTweetOptions options = new SendTweetOptions() { Status = "Hello from Bot!" };

            service.SendTweet(options);

            var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
            foreach (var tweet in tweets)
            {
                Console.WriteLine("{0} says '{1}'", tweet.User.ScreenName, tweet.Text);
            }//*/


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
    }
}
