using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Feed2Md.Feed
{
    public class Podcast
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string LastBuildDate { get; set; }
        public string ImageUrl { get; set; }
        public string Language { get; set; }
        public string Generator { get; set; }
        public List<Episode> Episodes { get; set; } = new List<Episode>();
        public PodcastItunes Itunes { get; set; } = new PodcastItunes();

        public override string ToString()
        {
            return $"[{Title}]({Link})";
        }
    }
}
