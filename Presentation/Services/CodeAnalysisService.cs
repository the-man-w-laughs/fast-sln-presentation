using System.Diagnostics;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Presentation.Models;
using Presentation.SyntaxWalkers;

namespace Presentation.Services;

public class CodeAnalysisService
{
    public CodeAnalysisService() { }

    public XmlDocument AnalyzeCodeFiles(List<ContentFile> allFiles)
    {
        var syntaxTrees = allFiles
            .Select(file => CSharpSyntaxTree.ParseText(file.Content))
            .ToList();

        var compilation = CSharpCompilation
            .Create("MyCompilation")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTrees);

        var stopwatchOverall = Stopwatch.StartNew();
        var xmlDocument = new XmlDocument();
        XmlElement rootElement = xmlDocument.CreateElement("root");
        xmlDocument.AppendChild(rootElement);

        foreach (var (tree, file) in syntaxTrees.Zip(allFiles, (tree, file) => (tree, file)))
        {
            var stopwatchTree = Stopwatch.StartNew();

            var root = tree.GetRoot();
            var semanticModel = compilation.GetSemanticModel(tree);

            var fileElement = xmlDocument.CreateElement("File");
            fileElement.SetAttribute("name", file.Path);
            rootElement.AppendChild(fileElement);

            var sourceCodeWalker = new SourceCodeToXmlWalker(semanticModel, root, fileElement);
            sourceCodeWalker.Parse();

            stopwatchTree.Stop();
            Console.WriteLine(
                $"Time taken for tree {file.Path}: {stopwatchTree.ElapsedMilliseconds} ms"
            );
        }

        stopwatchOverall.Stop();
        Console.WriteLine($"Total time taken: {stopwatchOverall.ElapsedMilliseconds} ms");

        return xmlDocument;
    }
}
