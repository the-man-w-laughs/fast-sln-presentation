using FastSlnPresentation.BLL.Models;
using FastSlnPresentation.BLL.Models.Graph.GraphData;
using FastSlnPresentation.BLL.Models.JsonModels.Edges;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;

namespace FastSlnPresentation.BLL.Contracts
{
    public interface IClassAnalysisService
    {
        public JsonGraph AnalyzeCodeFiles(List<ContentFile> allFiles);
    }
}
