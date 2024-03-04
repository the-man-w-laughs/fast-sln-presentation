using System.Diagnostics;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Presentation.Creators;
using Presentation.Models;
using Presentation.Models.Graph.GraphData;

namespace Presentation.Services;

public class CodeAnalysisService
{
    private readonly ISourceCodeToXmlWalkerCreator _sourceCodeToXmlWalkerCreator;

    public CodeAnalysisService(ISourceCodeToXmlWalkerCreator sourceCodeToXmlWalkerCreator)
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

        var nodes = new List<object>();
        var edges = new List<object>();

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

        return new JsonGraph(nodes, edges);
    }
}
