using Presentation.Models;
using Presentation.Models.SnlInfo;

namespace Presentation.Contracts
{
    public interface ISlnParser
    {
        SlnInfo GetSlnProjectInfos(string sln);
    }
}
