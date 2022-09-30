using System;
using System.Collections.Generic;
using System.Text;

namespace XamlU.Demo.GitHubLibrary.Models
{
    public class GitHubComment
    {
        public int id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string body { get; set; }
        public GitHubUser user { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class NewGitHubComment
    {
        public string body { get; set; }
    }
}
