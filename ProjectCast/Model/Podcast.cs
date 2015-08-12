using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectCast.Model
{
    public class Podcast
    {
        public string ImageURL { get; set; }

        public string FeedUrl { get; set; }
        public IEnumerable<Episode> EpisodesList { get; set; }

        public Podcast()
        { }

        public string GetLatestEpisode()
        {
            foreach (var episode in EpisodesList)
            {
                return episode.EpisodeUrl;
            }

            return "";
        }

        public async Task<IEnumerable<Episode>> GetEpisodeList()
        {
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync(this.FeedUrl);
            string resultString = await response.Content.ReadAsStringAsync();

            return HandleFeed(resultString);
        }

        private IEnumerable<Episode> HandleFeed(string feedString)
        {
            List<Episode> episodesList = new List<Episode>();

            try
            {
                // build XML DOM from feed string
                XDocument doc = XDocument.Parse(feedString);
                foreach (var item in doc.Descendants("item"))
                {
                    episodesList.Add(new Episode
                    {
                        Name = item.Element("title").Value,
                        EpisodeUrl = item.Element("enclosure").Attribute("url").Value
                    });
                }
            }
            catch (System.Exception)
            {
            }

            return episodesList;
        }
    }

    public class Episode
    {
        public string Name { get; set; }
        public string EpisodeUrl { get; set; }
    }
}