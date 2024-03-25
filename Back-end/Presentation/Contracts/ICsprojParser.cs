using Presentation.Models.Project;

namespace Presentation.Contracts
{
    public interface ICsprojParser
    {
        CsprojInfo GetProjectInfo(string csproj);
    }
}
