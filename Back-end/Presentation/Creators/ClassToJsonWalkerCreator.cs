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
        XmlElement xmlElement
    )
    {
        var idSerivice = new IdService();
        return new SourceToJsonWalker(semanticModel, root, xmlElement, idSerivice);
    }
}
