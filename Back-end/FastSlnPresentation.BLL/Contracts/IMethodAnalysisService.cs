using FastSlnPresentation.BLL.Models.Graph.GraphData;

namespace FastSlnPresentation.BLL.Contracts
{
    public interface IMethodAnalysisService
    {
        public JsonGraph AnalyzeStringAsync(string code);
    }
}
