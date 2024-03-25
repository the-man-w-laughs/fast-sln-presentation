using Moq;
using Presentation.Contracts;
using Presentation.Models.Project;

namespace Tests.ServiceTests.Helpers;

public class CsprojParserHelper
{
    private readonly Mock<ICsprojParser> _mock;

    public CsprojParserHelper(Mock<ICsprojParser> mock)
    {
        _mock = mock;
    }

    public void SetupGetSlnInfo(string csproj, CsprojInfo csprojInfo)
    {
        _mock.Setup(x => x.GetProjectInfo(csproj)).Returns(() => csprojInfo);
    }
}
