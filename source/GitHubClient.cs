using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XamlU.Demo.GitHubLibrary.Models;

namespace XamlU.Demo.GitHubLibrary
{
    /// <summary>
    /// Class GitHubClient.
    /// </summary>
    public class GitHubClient
    {
        private const string ApiBaseUrl = "https://api.github.com/";
        private static string _clientId;
        private static string _clientSecret;
        private static readonly HttpClient AuthClient;

        /// <summary>
        /// Initializes static members of the <see cref="GitHubClient"/> class.
        /// </summary>
        static GitHubClient()
        {
            AuthClient = new HttpClient();
        }

        /// <summary>
        /// get a token from a single-use code as an asynchronous operation.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="code">The code.</param>
        /// <returns>Task.</returns>
        public async Task GetTokenFromCodeAsync(string clientId, string clientSecret, string code)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            var endPoint =
                $"https://github.com/login/oauth/access_token?client_id={_clientId}&client_secret={_clientSecret}&code={code}";
            var response = await AuthClient.PostAsync(endPoint, new StringContent(string.Empty));
            response.EnsureSuccessStatusCode();

            // response is a string similar to token=<tokenhere>&scope=&
            var tokens = await response.Content.ReadAsStringAsync();
            var splitTokens = tokens.Split('&');
            var token = splitTokens[0].Split('=')[1];
            AuthClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// get the current authenticated user details as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;GitHubUser&gt;.</returns>
        public async Task<GitHubUser> GetUserDetailsAsync()
        {
            var api = BuildApi("user");
            var request = BuildGetRequest(api);
            var result = await GetStringResult(request);
            try
            {
                return JsonConvert.DeserializeObject<GitHubUser>(result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// search respositories as an asynchronous operation.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;GitHubSearchRepositoriesResults&gt;.</returns>
        public async Task<GitHubSearchRepositoriesResults> SearchRespositoriesAsync(string query)
        {
            var api = BuildApi("search/repositories", new Dictionary<string, string> {{"q", query}});
            var request = BuildGetRequest(api);
            var result = await GetStringResult(request);
            try
            {
                return JsonConvert.DeserializeObject<GitHubSearchRepositoriesResults>(result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// get a repository as an asynchronous operation.
        /// </summary>
        /// <param name="fullRepositoryName">Name of the repository. This should be the 'full_name' not the 'name' and is of the format "username/repositoryName".</param>
        /// <returns>Task&lt;GitHubRepository&gt;.</returns>
        public async Task<GitHubRepository> GetRepositoryAsync(string fullRepositoryName)
        {
            var api = BuildApi($"repos/{fullRepositoryName}");
            var request = BuildGetRequest(api);
            var result = await GetStringResult(request);
            try
            {
                return JsonConvert.DeserializeObject<GitHubRepository>(result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// get repository issues as an asynchronous operation.
        /// </summary>
        /// <param name="fullRepositoryName">Name of the repository. This should be the 'full_name' not the 'name' and is of the format "username/repositoryName".</param>
        /// <returns>Task&lt;GitHubIssues&gt;.</returns>
        public async Task<GitHubIssues> GetRepositoryIssuesAsync(string fullRepositoryName)
        {
            var api = BuildApi($"repos/{fullRepositoryName}/issues", 
                      new Dictionary<string, string> { { "state", "all" } });
            var request = BuildGetRequest(api);
            var result = await GetStringResult(request);
            try
            {
                return new GitHubIssues { Issues = JsonConvert.DeserializeObject<GitHubIssue[]>(result) };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// get repository issues as an asynchronous operation.
        /// </summary>
        /// <param name="fullRepositoryName">Name of the repository. This should be the 'full_name' not the 'name' and is of the format "username/repositoryName".</param>
        /// <param name="issueId">The issue id.</param>
        /// <returns>Task&lt;GitHubIssues&gt;.</returns>
        public async Task<GitHubComment[]> GetRepositoryIssueCommentsAsync(string fullRepositoryName, int issueId)
        {
            var api = BuildApi($"repos/{fullRepositoryName}/issues/{issueId}/comments");
            var request = BuildGetRequest(api);
            var result = await GetStringResult(request);
            try
            {
                return JsonConvert.DeserializeObject<GitHubComment[]>(result);
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// post repository issue as an asynchronous operation.
        /// </summary>
        /// <param name="fullRepositoryName">Name of the repository. This should be the 'full_name' not the 'name' and is of the format "username/repositoryName".</param>
        /// <param name="issue">The issue id.</param>
        /// <returns>Task&lt;GitHubCreateIssueResponse&gt;.</returns>
        public async Task<GitHubCreateIssueResponse> PostRepositoryIssueAsync(string fullRepositoryName,
            GitHubCreateIssue issue)
        {
            var api = BuildApi($"repos/{fullRepositoryName}/issues");
            var serializeObject = JsonConvert.SerializeObject(issue);
            var request = BuildPostRequest(api, serializeObject);
            var result = await GetStringResult(request);
            try
            {
                return JsonConvert.DeserializeObject<GitHubCreateIssueResponse>(result);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// post repository comment as an asynchronous operation.
        /// </summary>
        /// <param name="fullRepositoryName">Name of the repository. This should be the 'full_name' not the 'name' and is of the format "username/repositoryName".</param>
        /// <param name="issueId">The issue id.</param>
        /// <param name="comment">The comment body.</param>
        /// <returns>Task&lt;GitHubComment&gt;.</returns>
        public async Task<GitHubComment> PostRepositoryIssueCommentAsync(string fullRepositoryName, int issueId,
            string comment)
        {
            var api = BuildApi($"repos/{fullRepositoryName}/issues/{issueId}/comments");
            var serializeObject = JsonConvert.SerializeObject(new NewGitHubComment{ body = comment });
            var request = BuildPostRequest(api, serializeObject);
            var result = await GetStringResult(request);
            try
            {
                return JsonConvert.DeserializeObject<GitHubComment>(result);
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// list grants as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> ListGrantsAsync()
        {
            var api = BuildApi("applications/grants");
            var request = BuildGetRequest(api);
            return await GetStringResult(request);
        }

        /// <summary>
        /// Adds the secrets.
        /// </summary>
        /// <param name="queries">The queries.</param>
        private void AddSecrets(IDictionary<string, string> queries)
        {
            queries.Add("client_id", _clientId);
            queries.Add("client_secret", _clientSecret);
        }

        /// <summary>
        /// Builds the API.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="queries">The queries.</param>
        /// <returns>System.String.</returns>
        private string BuildApi(string api, Dictionary<string, string> queries = null)
        {
            if (queries == null)
                queries = new Dictionary<string, string>();
            return $"{ApiBaseUrl}{api}{BuildQueryString(queries)}";
        }

        /// <summary>
        /// Builds the get request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>HttpRequestMessage.</returns>
        private HttpRequestMessage BuildGetRequest(string url)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue("UWP-GitHub-Browser", "1.0"));

            return request;
        }

        /// <summary>
        /// Builds the post request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="content">The content.</param>
        /// <returns>HttpRequestMessage.</returns>
        private HttpRequestMessage BuildPostRequest(string url, string content)
        {
            var request = BuildGetRequest(url);
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(content);
            return request;
        }

        /// <summary>
        /// Builds the query string.
        /// </summary>
        /// <param name="queries">The queries.</param>
        /// <returns>System.String.</returns>
        private string BuildQueryString(Dictionary<string, string> queries = null)
        {
            if (queries == null)
                queries = new Dictionary<string, string>();
            AddSecrets(queries);
            var sb = new StringBuilder("?");
            foreach (var queryPair in queries)
                sb.Append($"{queryPair.Key}={queryPair.Value ?? string.Empty}&");

            return sb.ToString();
        }

        /// <summary>
        /// Gets the string result.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        private static async Task<string> GetStringResult(HttpRequestMessage request)
        {
            var responseMessage = await AuthClient.SendAsync(request);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsStringAsync();
        }
    }
}