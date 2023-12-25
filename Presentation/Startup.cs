using Microsoft.CodeAnalysis.CSharp;

class SolutionParser
{
    // static async Task Main()
    // {
    //     var solutionParser = new SlnParser();
    //     var projectParser = new CsprojParser();

    //     var pat = ConfigurationManager.AppSettings.Get("pat");
    //     var githubService = new GithubService(pat);

    //     var stopwatch = Stopwatch.StartNew();
    //     var allFiles = await githubService.GetAllFiles("the-man-w-laughs", "Obj-Renderer");
    //     Console.WriteLine(stopwatch.ElapsedMilliseconds);
    //     Console.WriteLine(allFiles.Count);

    //     var contentFileService = new ContentFileService(solutionParser, projectParser);

    //     var slnTrees = contentFileService.GetSnlTrees(allFiles);
    // }

    // static async Task Main()
    // {
    //     var codeAnalysisService = new CodeAnalysisService();
    //     string code =
    //         @"
    //         using System;

    //         class Program
    //         {
    //             static void Main()
    //             {
    //                 Console.WriteLine(""Hello, World!"");
    //             }
    //         }
    //     ";
    //     var block = codeAnalysisService.AnalyzeCode(code);
    // }


    static void Main()
    {
        var code =
            @" 
            public interface IMyInterface { void Method1(); }

            public class MyBaseClass { }

            public class MyClass : MyBaseClass, IMyInterface
            {
                public void Method1() { }
            }
        ";

        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        var root = syntaxTree.GetRoot();

        // var walker = new ClassDiagramGenerator();
        // walker.Visit(root);

        // walker.PrintClassDiagram();
    }
}
