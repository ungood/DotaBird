using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace DotaBird.Core.Twitter
{
    /// <summary>
    /// Read from a file called RequestTable, return results in a dictionary format.
    /// </summary>
    public class PlayerRequests
    {
        public Dictionary<long, string> GetRequests()
        {
            Dictionary<long, string> requestDictionary = new Dictionary<long, string>();
            FileInfo fi = new FileInfo(@"c:\Users\Wovoka\Documents\GitHub\DotaBird\RequestTable.txt");
            StreamReader reader = fi.OpenText();
            string line;

            line = reader.ReadLine();   // skips the first line

            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split('\t');
                long playerRequested = Convert.ToInt64(items[0]);
                requestDictionary.Add(playerRequested, items[1]);
            }

            reader.Close();
            return requestDictionary;
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
