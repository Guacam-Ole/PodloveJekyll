

namespace Feed2Md.Feed
{
    public class PodcastItunes
    {
        public string ImageUrl { get; set; }
        public AppleItunesPerson Owner { get; set; } = new AppleItunesPerson();
        public string SubTitle { get; set; }
        public bool Explicit { get; set; }
        public string Category { get; set; }
        public string RssType { get;set;}
        public string Author { get; set; }
        public bool Block { get; set; }
        public string Summary { get; set; }
       
    }
}
