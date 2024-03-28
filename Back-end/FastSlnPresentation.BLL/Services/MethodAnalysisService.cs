using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using FastSlnPresentation.BLL.Models.Graph.GraphData;
using FastSlnPresentation.BLL.Models.JsonModels.Edges;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using FastSlnPresentation.BLL.SyntaxWalkers;
using FastSlnPresentation.BLL.Contracts;

namespace FastSlnPresentation.BLL.Services;

public class MethodAnalysisService : IMethodAnalysisService
{
    private readonly IIdService _idService;

    public MethodAnalysisService(IIdService idService)
    {
        _idService = idService;
    }

    public JsonGraph AnalyzeStringAsync(string code)
    {
        var stopwatchOverall = Stopwatch.StartNew();

        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        var nodes = new List<INode>();
        var edges = new List<JsonEdge>();

        var root = syntaxTree.GetRoot();

        var classToJsonWalker = new MethodToJsonWalker(nodes, edges, root, _idService);

        classToJsonWalker.Parse();

        Console.WriteLine($"Total time taken: {stopwatchOverall.ElapsedMilliseconds} ms");

        return new JsonGraph(nodes.Cast<object>().ToList(), edges.Cast<object>().ToList());
    }
}
