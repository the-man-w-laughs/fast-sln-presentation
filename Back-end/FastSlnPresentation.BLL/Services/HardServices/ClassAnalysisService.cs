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
        }

        HashSet<string> nodeIds = new HashSet<string>(nodes.Select(node => node.Id));

        var existingEdges = edges
            .Where(edge => nodeIds.Contains(edge.Source) && nodeIds.Contains(edge.Target))
            .ToList();

        var nonExistingSources = edges.Select(edge => edge.Source).Except(nodeIds).ToList();

        var nonExistingTargets = edges.Select(edge => edge.Target).Except(nodeIds).ToList();

        var nonExistingSourcesAndTargets = nonExistingSources.Concat(nonExistingTargets).ToList();

        string filePath = "/home/nazar/Documents/zalupa0-1.txt";

        // Write the strings to the file
        File.WriteAllLines(filePath, nonExistingSourcesAndTargets);

        return new JsonGraph(nodes.Cast<object>().ToList(), existingEdges.Cast<object>().ToList());
    }
}
