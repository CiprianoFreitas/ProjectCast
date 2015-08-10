using ProjectCast.Model;
using System.Collections.Generic;

namespace ProjectCast.Helpers
{
    internal interface IPodcastService
    {
        IEnumerable<Podcast> GetPodcasts(string searchTerm);
    }
}