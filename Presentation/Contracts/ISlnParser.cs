using Presentation.Models.SnlInfo;

namespace Presentation.Contracts
{
    public interface ISlnParser
    {
        SlnInfo GetSlnInfo(string sln);
    }
}
