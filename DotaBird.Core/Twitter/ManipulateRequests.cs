using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using TweetSharp;

namespace DotaBird.Core.Twitter
{
    /// <summary>
    /// Manipulate text file from a requestor calling this service on twitter in order to sign up or to remove themselves
    /// *format of file: first line = (long) last tweet ID (newLine)
    /// *All other lines = playerRequested (tab) requestor (newLine)
    /// *title of file: RequestTable
    /// **Format of tweet:
    /// *for adding: Add me! (space) (Player ID to be added)
    /// *for deleting: Remove me! 
    /// </summary>
    public class ManipulateRequests
    {

        public ManipulateRequests(TwitterHandler twitterHandler)
        {
            FileInfo fi = new FileInfo(@"c:\Users\Wovoka\Documents\GitHub\DotaBird\RequestTable.txt");
            StreamReader reader = fi.OpenText();
            string line, file;

            line = reader.ReadLine();
            file = reader.ReadToEnd();
            reader.Close();

            long last = Convert.ToInt64(line);
            var tweetList = twitterHandler.ReadTimeLine(last);

            StreamWriter writer = new StreamWriter(fi.FullName);
            writer.WriteLine(tweetList.Last().Id.ToString());
            writer.Write(file);
            writer.Close();

            var tweets = tweetList.GetEnumerator();

            while (tweets.MoveNext())
            {
                var tweet = tweets.Current;

                if (tweet.Text.Contains("Add me!"))
                    AddRequest(tweet);

                if (tweet.Text.Contains("Remove me!"))
                    DeleteRequest(tweet);
            }
        }
        public void AddRequest(TwitterStatus tweet)
        {
            /// Need to add robust input checking. Also need to check if the playerID requested is accessible            
            string[] lines = tweet.Text.Split('!');
            lines[1] = lines[1].TrimStart();

            using (StreamWriter writer = File.AppendText(@"c:\Users\Wovoka\Documents\GitHub\DotaBird\RequestTable.txt"))
            {
                writer.Write(lines[1]);
                writer.Write('\t');
                writer.Write(tweet.User.ScreenName);
            }
        }

        public void DeleteRequest(TwitterStatus tweet)
        {
            FileInfo fi = new FileInfo(@"c:\Users\Wovoka\Documents\GitHub\DotaBird\RequestTable.txt");
            StreamReader reader = fi.OpenText();
            Stack<string> lines = new Stack<string>();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (!line.Contains(tweet.User.ScreenName))
                    lines.Push(line);
            }
            reader.Close();

            StreamWriter writer = new StreamWriter(fi.FullName);
            
            while ((line = lines.Pop()) != null)
                writer.WriteLine(line);

            writer.Close();
        }
    }
}
