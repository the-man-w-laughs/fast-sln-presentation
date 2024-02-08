using Moq;
using Presentation.Contracts;
using Presentation.Models.SnlInfo;

namespace Tests.ServiceTests.Helpers;

public class SlnParserHelper
{
    private readonly Mock<ISlnParser> _mock;

    public SlnParserHelper(Mock<ISlnParser> mock)
    {
        _mock = mock;
    }

    public void SetupGetSlnInfo(string sln, SlnInfo slnInfo)
    {
        _mock.Setup(x => x.GetSlnInfo(sln)).Returns(() => slnInfo);
    }
}
