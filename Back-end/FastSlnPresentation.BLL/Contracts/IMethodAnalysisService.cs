using FastSlnPresentation.BLL.Models;
using FastSlnPresentation.BLL.Models.Graph.GraphData;
using FastSlnPresentation.BLL.Models.JsonModels.Edges;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FastSlnPresentation.BLL.Contracts
{
    public interface IMethodAnalysisService
    {
        public JsonGraph AnalyzeStringAsync(string code);
    }
}
