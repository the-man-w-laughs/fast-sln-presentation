using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using FastSlnPresentation.BLL.Models;
using FastSlnPresentation.BLL.Models.Graph.GraphData;
using FastSlnPresentation.BLL.Models.JsonModels.Edges;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using FastSlnPresentation.BLL.SyntaxWalkers;
using FastSlnPresentation.BLL.Contracts;

namespace FastSlnPresentation.BLL.Services;

public class ClassAnalysisService : IClassAnalysisService
{
    private readonly IIdService _idService;

    public ClassAnalysisService(IIdService idService)
    {
        _idService = idService;
    }

    public JsonGraph AnalyzeCodeFiles(List<ContentFile> allFiles)
    {
        var stopwatchOverall = Stopwatch.StartNew();

        var syntaxTrees = allFiles
            .Select(file => CSharpSyntaxTree.ParseText(file.Content))
            .ToList();

        var compilation = CSharpCompilation
            .Create("MyCompilation")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTrees);

        var nodes = new List<INode>();
        var edges = new List<JsonEdge>();

        foreach (var (tree, file) in syntaxTrees.Zip(allFiles, (tree, file) => (tree, file)))
        {
            var stopwatchTree = Stopwatch.StartNew();
            var semanticModel = compilation.GetSemanticModel(tree);
            var root = tree.GetRoot();

            var classToJsonWalker = new ClassToJsonWalker(
                nodes,
                edges,
                semanticModel,
                root,
                _idService
            );

            classToJsonWalker.Parse();

            stopwatchTree.Stop();
            Console.WriteLine(
                $"Time taken for tree {file.Path}: {stopwatchTree.ElapsedMilliseconds} ms"
            );
        }

        stopwatchOverall.Stop();
        Console.WriteLine($"Total time taken: {stopwatchOverall.ElapsedMilliseconds} ms");

        return new JsonGraph(nodes.Cast<object>().ToList(), edges.Cast<object>().ToList());
    }
}
