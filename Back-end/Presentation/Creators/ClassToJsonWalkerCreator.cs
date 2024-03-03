using System.Xml;
using Microsoft.CodeAnalysis;
using Presentation.Contracts;
using Presentation.Services;
using Presentation.SyntaxWalkers;

namespace Presentation.Creators;

public class ClassToJsonWalkerCreator : ISourceCodeToXmlWalkerCreator
{
    public ISourceCodeToXmlWalker Create(
        SemanticModel semanticModel,
        SyntaxNode root,
        List<object> nodes,
        List<object> edges
    )
    {
        var idSerivice = new IdService();
        var modifiersMappingHelper = new ModifiersMappingHelper();

        return new SourceToJsonWalker(
            nodes,
            edges,
            semanticModel,
            root,
            idSerivice,
            modifiersMappingHelper
        );
    }
}
