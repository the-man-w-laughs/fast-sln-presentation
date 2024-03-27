using FastSlnPresentation.BLL.Models.SnlInfo;

namespace FastSlnPresentation.BLL.Contracts
{
    public interface ISlnParser
    {
        SlnInfo GetSlnInfo(string sln);
    }
}
