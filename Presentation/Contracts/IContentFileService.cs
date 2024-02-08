using Presentation.Models;
using Presentation.Models.SnlTree;

namespace Presentation.Contracts
{
    public interface IContentFileService
    {
        List<SlnTree> GetSnlTrees(IEnumerable<ContentFile> contentFiles);
    }
}
