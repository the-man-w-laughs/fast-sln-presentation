using Presentation.Models.Project;

namespace Presentation.Contracts
{
    public interface ICsprojParser
    {
        ProjectInfo GetProjectInfo(string csproj);
    }
}
