using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using System;

namespace Feed2Md
{
    class Program
    {
        static void Main(string[] args)
        {
            var startup = new Startup();
            var services = new ServiceCollection();
            services.AddLogging(opt => opt.AddConsole());
            startup.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var import=serviceProvider.GetService<ImportRss>();
            var markdown = serviceProvider.GetService<Markdown>();

            var podcast=import.ImportRssFile();
            markdown.CreateMarkdownFiles(podcast);
        }
    }
}
