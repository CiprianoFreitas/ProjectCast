using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UWAfirst.Model
{
    public class Podcast
    {
        public string ImageURL { get; set; }
        public string FeedUrl { get; set; }

        public Podcast()
        {

        }

        public string GetLatestEpisode()
        {
            string sURL;
            sURL = FeedUrl;

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            Stream objStream;
            objStream = wrGETURL.GetResponseAsync().GetAwaiter().GetResult().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string response = "";
            string line;
            while ((line = objReader.ReadLine()) != null)
            {
                response += line;
            }
            return HandleFeed(response);
        }

        private string HandleFeed(string feedString)
        {
            // build XML DOM from feed string
            XDocument doc = XDocument.Parse(feedString);
            foreach (var item in doc.Descendants("item"))
            {
                return item.Element("enclosure").Attribute("url").Value;
            }
            return "";
        }

    }
}
