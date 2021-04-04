using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feed2Md.Feed
{
    public class Episode
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Published { get; set; }
        public DateTime PublishingDate
        {
            get
            {
                if (Published == null) return new DateTime();
                string parseFormat = "ddd, dd MMM yyyy HH:mm:ss zzz";
                return DateTime.ParseExact(Published, parseFormat, CultureInfo.InvariantCulture);
            }
        }
        // Todo: add getter as datetime
        public string Guid { get; set; }
        public string Description { get; set; }
        public string AudioUrl { get; set; }
        public int AudioLength { get; set; }
        public List<Chapter> Chapters { get; set; } = new List<Chapter>();
        public EpisodeItunes Itunes { get; set; } = new EpisodeItunes();
        public override string ToString()
        {
            return $"[{Title}]({Link})";
        }

    }
}
