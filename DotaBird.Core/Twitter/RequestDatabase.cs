using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using TweetSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotaBird.Core.Twitter
{
    /// <summary>
    /// Manipulate json file from a requestor calling this service on twitter in order to sign up or to remove themselves
    /// *format of file:
    /// { "lastTweetId": (a long),
    /// "tweets": [ { "userName": (a string), "playerRequested": (a long; id)}, etc... ] }
    /// *title of file: requests.txt
    /// **Format of tweet:
    /// *for adding: Add me! (space) (Player ID to be added)
    /// *for deleting: Remove me! 
    /// </summary>
    public class RequestDatabase
    {
        public PlayerRequests requests { get; set; }

        public void GetRequestsFromTwitter(TwitterHandler twitterHandler)
        {
            // put content from requests.txt into a string
            FileInfo fi = new FileInfo(@"c:\Users\Wovoka\Documents\GitHub\DotaBird\requests.txt");
            StreamReader reader = fi.OpenText();
            string json = reader.ReadToEnd();
            reader.Close();

            requests = JsonConvert.DeserializeObject<PlayerRequests>(json);  // converts string into json object

            var tweetList = twitterHandler.ReadTimeLine(requests.LastTweetId);

            requests.LastTweetId = tweetList.Last().Id;     // changes lastTweetId

            // check the tweets from timeline, add/remove from json object
            var tweets = tweetList.GetEnumerator();
            while (tweets.MoveNext())
            {
                var tweet = tweets.Current;

                if (tweet.Text.Contains("Add me!"))
                    AddRequest(tweet);

                if (tweet.Text.Contains("Remove me!"))
                    DeleteRequest(tweet);
            }
            
            // write the modified json object to file
            json = JsonConvert.SerializeObject(requests);
            using (StreamWriter writer = new StreamWriter(fi.FullName))
                writer.Write(json);

        }

        private void AddRequest(TwitterStatus tweet)
        {
            /// Need to add robust input checking. Also need to check if the playerID requested is accessible            
            string[] lines = tweet.Text.Split('!');
            lines[1] = lines[1].TrimStart();

            Requestor newRequestor = new Requestor();
            newRequestor.PlayerRequested = Convert.ToInt64(lines[1]);
            newRequestor.UserName = tweet.User.ScreenName;

            JObject rss = JObject.Parse(requests.ToString());
            JArray requestors = (JArray)rss["requestors"];
            requestors.Add(newRequestor);

            requests = JsonConvert.DeserializeObject<PlayerRequests>(rss.ToString());
        }

        private void DeleteRequest(TwitterStatus tweet)
        {
            JObject rss = JObject.Parse(requests.ToString());
            JArray requestors = (JArray)rss["requestors"];
            requestors.Remove(tweet.User.ScreenName);      // need to test this

            requests = JsonConvert.DeserializeObject<PlayerRequests>(rss.ToString());
        }

        public string ConvertPlayerIDToName(long steamId)
        {
            throw new NotImplementedException();
        }

        public bool IsPlayerRequestedAccessable()
        {
            throw new NotImplementedException();
        }
    }
}
