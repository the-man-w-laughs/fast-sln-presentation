using FastSlnPresentation.BLL.Models;
using FastSlnPresentation.BLL.Models.Graph.GraphData;

namespace FastSlnPresentation.BLL.Contracts
{
    public interface IClassAnalysisService
    {
        public JsonGraph AnalyzeCodeFiles(List<ContentFile> allFiles);
    }
}
