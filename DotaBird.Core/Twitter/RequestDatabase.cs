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
    /// "requestors": [ { "userName": (a string), "playerRequested": (a long; id)}, etc... ] }
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
            
            tweetList = tweetList.ToList<TwitterStatus>();

            if (tweetList.Any())                                // if tweetList is not empty
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

            requests.Requestors.Add(newRequestor);
        }

        private void DeleteRequest(TwitterStatus tweet) 
        {
            foreach (Requestor current in requests.Requestors)  
            {
                if (current.UserName == tweet.User.ScreenName)
                {
                    requests.Requestors.Remove(current);
                    break;
                }
            }      
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
