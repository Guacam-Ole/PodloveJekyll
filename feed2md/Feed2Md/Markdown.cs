using Feed2Md.Feed;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feed2Md
{
    public class Markdown
    {
        TemplateGlobal _globalTemplateVariables = new TemplateGlobal();
        private readonly IConfiguration _configuration;

        public Markdown(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CreateMarkdownFiles(Podcast podcast)
        {
            CreateEpisodePages(ref podcast);
            CreateMainPage(podcast);
        }

        private void GetExportConfig(string subsection, out string path, out string file)
        {
            var exportConfig = _configuration.GetSection("export").GetSection(subsection);
            path = exportConfig.GetValue<string>("path");
            file = exportConfig.GetValue<string>("file");
        }

        private void CreateMainPage(Podcast podcast)
        {
            GetExportConfig("podcast", out string targetPath, out string targetFile);
            if (string.IsNullOrEmpty(targetPath) || string.IsNullOrEmpty(targetFile)) return;

            var podcastTemplate = File.ReadAllText("./templates/podcast.md");

            string tokenizedContent = TokenHelper.ReplaceTokens(podcastTemplate, podcast, "Podcast.");
            tokenizedContent = TokenHelper.ReplaceTokens(tokenizedContent, _globalTemplateVariables, "Global.");
            var targetFileContents=tokenizedContent.CleanString();

            targetPath = TokenHelper.ReplaceTokens(targetPath, podcast, "Podcast.").CleanString(CleanMode.InPath);
            targetFile = TokenHelper.ReplaceTokens(targetFile, podcast, "Podcast.").CleanString(CleanMode.InFile);

            Directory.CreateDirectory(targetPath);

            File.WriteAllText(Path.Combine(targetPath, targetFile), targetFileContents);
        }

        private void CreateEpisodePages(ref Podcast podcast)
        {
            GetExportConfig("episode", out string targetPath, out string targetFile);
            if (string.IsNullOrEmpty(targetPath) || string.IsNullOrEmpty(targetFile)) return;
            var podcastTemplate = File.ReadAllText("./templates/episode.md");

            foreach (var episode in podcast.Episodes)
            {
                string targetFileContents;
                string episodePath = TokenHelper.ReplaceTokens(targetPath, episode, "Episode.").CleanString(CleanMode.InPath);
                string episodeFile = TokenHelper.ReplaceTokens(targetFile, episode, "Episode.").CleanString(CleanMode.InFile);

                Directory.CreateDirectory(episodePath);
                string newLink = Path.Combine(episodePath, episodeFile);
                episode.Link = Path.Combine(_configuration.GetSection("baseurl").Value, newLink);

                string tokenizedContent = TokenHelper.ReplaceTokens(podcastTemplate, episode, "Episode.");
                tokenizedContent = TokenHelper.ReplaceTokens(tokenizedContent, podcast, "Podcast.");
                tokenizedContent = TokenHelper.ReplaceTokens(tokenizedContent, _globalTemplateVariables, "Global.");
                targetFileContents = tokenizedContent.CleanString();

                File.WriteAllText(newLink, targetFileContents);
            }
        }
    }
}
