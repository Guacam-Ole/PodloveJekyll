using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feed2Md.Feed
{
    public class Chapter
    {
        public string Start { get; set; }
        // todo: Add Getter as timespan
        public string Title { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return $"[{Start}] {Title}";
        }
    }
}
