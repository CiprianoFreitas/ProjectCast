using Newtonsoft.Json.Linq;
using ProjectCast.Model;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ProjectCast.Helpers
{
    public class ItunesService : IPodcastService
    {
        public IEnumerable<Podcast> GetPodcasts(string searchTerm)
        {
            List<Podcast> podcastList = new List<Podcast>();
            string sURL = string.Format("http://itunes.apple.com/search?term={0}&media=podcast", searchTerm);

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            wrGETURL.ContentType = "application/json";
            Stream objStream;
            objStream = wrGETURL.GetResponseAsync().GetAwaiter().GetResult().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string response = "";
            string line;
            while ((line = objReader.ReadLine()) != null)
            {
                response += line;
            }

            JToken token = JObject.Parse(response);

            int resultsCount = (int)token.SelectToken("resultCount");

            var results = (IEnumerable<dynamic>)token.SelectToken("results");

            foreach (var result in results)
            {
                podcastList.Add(new Podcast
                {
                    ImageURL = (string)result.SelectToken("artworkUrl600"),
                    FeedUrl = (string)result.SelectToken("feedUrl")
                });
            }

            return podcastList;
        }
    }
}