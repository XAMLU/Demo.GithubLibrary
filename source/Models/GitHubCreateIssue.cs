namespace XamlU.Demo.GitHubLibrary.Models
{
    public class GitHubCreateIssue
    {
        public string title { get; set; }
        public string body { get; set; }
        public string assignee { get; set; }
        public int? milestone { get; set; }
        public string[] labels { get; set; }
    }
}
