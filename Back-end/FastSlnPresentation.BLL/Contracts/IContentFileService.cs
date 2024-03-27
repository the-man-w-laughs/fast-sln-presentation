using FastSlnPresentation.BLL.Models;
using FastSlnPresentation.BLL.Models.SnlTree;

namespace FastSlnPresentation.BLL.Contracts
{
    public interface IContentFileService
    {
        List<SlnTree> GetSnlTrees(IEnumerable<ContentFile> contentFiles);
    }
}
