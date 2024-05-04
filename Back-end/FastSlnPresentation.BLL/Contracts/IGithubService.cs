using FastSlnPresentation.BLL.Models;

namespace FastSlnPresentation.BLL.Contracts
{
    public interface IGithubService
    {
        public Task<List<ContentFile>> GetAllFiles(string owner, string repoName);
    }
}
