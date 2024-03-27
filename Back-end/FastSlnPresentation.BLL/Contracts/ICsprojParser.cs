using FastSlnPresentation.BLL.Models.Project;

namespace FastSlnPresentation.BLL.Contracts
{
    public interface ICsprojParser
    {
        CsprojInfo GetProjectInfo(string csproj);
    }
}
