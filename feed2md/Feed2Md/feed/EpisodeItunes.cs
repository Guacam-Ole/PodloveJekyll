using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feed2Md.Feed
{
    public class EpisodeItunes
    {
        public string Duration { get; set; }
        // Todo: add getter as timespan
        public string Author { get; set; }
        public string SubTitle { get; set; }
        public string Episode { get; set; }
        public string EpisodeType { get; set; }
        public string Summary { get; set; }
        public string Season { get; set; }
    }
}
