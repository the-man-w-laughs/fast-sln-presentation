using Presentation.Models;

namespace Presentation.Contracts
{
    public interface IGithubService
    {
        public Task<List<ContentFile>> GetAllFiles(string owner, string repoName);
    }
}
