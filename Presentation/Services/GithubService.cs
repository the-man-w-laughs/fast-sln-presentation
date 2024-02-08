using Octokit;
using Presentation.Contracts;
using Presentation.Models;
using System.Text;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace Business.Octokit
{
    public class GithubService : IGithubService
    {
        private GitHubClient _client = new GitHubClient(
            new ProductHeaderValue("fast-snl-presentation")
        );

        public GithubService() { }

        public GithubService(string pat)
        {
            var basicAuth = new Credentials(pat);
            _client.Credentials = basicAuth;
        }

        public async Task<List<ContentFile>> GetAllFiles(string owner, string repoName)
        {
            var repo = await GetRepository(owner, repoName);
            var branchSha = await GetBranchSha(repo.Id, repo.DefaultBranch);
            var tree = await _client.Git.Tree.GetRecursive(owner, repoName, branchSha);

            var allFiles = new List<ContentFile>();
            var tasks = new List<Task<ContentFile>>();

            foreach (var item in tree.Tree)
            {
                if (item.Type == TreeType.Blob)
                {
                    var path = item.Path;
                    var sha = item.Sha;
                    tasks.Add(
                        GetContent(owner, repoName, sha)
                            .ContinueWith(task => new ContentFile(path, task.Result))
                    );
                }
            }

            var results = await Task.WhenAll(tasks);
            allFiles.AddRange(results);

            return allFiles.ToList();
        }

        private async Task<string> GetContent(string owner, string repoName, string sha)
        {
            var blob = await _client.Git.Blob.Get(owner, repoName, sha);
            return Encoding.UTF8.GetString(Convert.FromBase64String(blob.Content));
        }

        private async Task<string> GetBranchSha(long id, string branchName)
        {
            var branch = await _client.Repository.Branch.Get(id, branchName);
            return branch.Commit.Sha;
        }

        private async Task<Repository> GetRepository(string owner, string repoName)
        {
            return await _client.Repository.Get(owner, repoName);
        }
    }
}
