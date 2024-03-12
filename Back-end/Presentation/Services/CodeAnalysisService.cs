using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Presentation.Creators;
using Presentation.Models;
using Presentation.Models.Graph.GraphData;
using Presentation.Models.JsonModels.Edges;
using Presentation.Models.JsonModels.Nodes;

namespace Presentation.Services;

public class CodeAnalysisService
{
    private readonly ISourceCodeToJsonWalkerCreator _sourceCodeToXmlWalkerCreator;

    public CodeAnalysisService(ISourceCodeToJsonWalkerCreator sourceCodeToXmlWalkerCreator)
    {
        _sourceCodeToXmlWalkerCreator = sourceCodeToXmlWalkerCreator;
    }

    public JsonGraph AnalyzeCodeFiles(List<ContentFile> allFiles)
    {
        var syntaxTrees = allFiles
            .Select(file => CSharpSyntaxTree.ParseText(file.Content))
            .ToList();

        var compilation = CSharpCompilation
            .Create("MyCompilation")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTrees);

        var stopwatchOverall = Stopwatch.StartNew();

        var nodes = new List<INode>();
        var edges = new List<JsonEdge>();

        foreach (var (tree, file) in syntaxTrees.Zip(allFiles, (tree, file) => (tree, file)))
        {
            var stopwatchTree = Stopwatch.StartNew();

            var root = tree.GetRoot();
            var semanticModel = compilation.GetSemanticModel(tree);

            var sourceCodeWalker = _sourceCodeToXmlWalkerCreator.Create(
                semanticModel,
                root,
                nodes,
                edges
            );

            sourceCodeWalker.Parse();

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
