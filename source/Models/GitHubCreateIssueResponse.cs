using System;

namespace XamlU.Demo.GitHubLibrary.Models
{
    public class GitHubCreateIssueResponse
    {
        public int id { get; set; }
        public string url { get; set; }
        public string repository_url { get; set; }
        public string labels_url { get; set; }
        public string comments_url { get; set; }
        public string events_url { get; set; }
        public string html_url { get; set; }
        public int number { get; set; }
        public string state { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public User user { get; set; }
        public Label[] labels { get; set; }
        public Assignee assignee { get; set; }
        public Milestone milestone { get; set; }
        public bool locked { get; set; }
        public int comments { get; set; }
        public Pull_Request pull_request { get; set; }
        public object closed_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Closed_By closed_by { get; set; }
    }

    public class Milestone
    {
        public string url { get; set; }
        public string html_url { get; set; }
        public string labels_url { get; set; }
        public int id { get; set; }
        public int number { get; set; }
        public string state { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Creator creator { get; set; }
        public int open_issues { get; set; }
        public int closed_issues { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime closed_at { get; set; }
        public DateTime due_on { get; set; }
    }

    public class Creator
    {
        public string login { get; set; }
        public int id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }

    public class Closed_By
    {
        public string login { get; set; }
        public int id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
    }


}
