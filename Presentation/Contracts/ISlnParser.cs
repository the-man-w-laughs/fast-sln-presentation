using Presentation.Models;

namespace Presentation.Contracts
{
    public interface ISlnParser
    {
        IEnumerable<SlnProjectInfo> GetSlnProjectInfos(string sln);
    }
}
