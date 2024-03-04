using System.Xml;
using Microsoft.CodeAnalysis;
using Presentation.Contracts;

namespace Presentation.Creators
{
    public interface ISourceCodeToXmlWalkerCreator
    {
        ISourceCodeToJsonWalker Create(
            SemanticModel semanticModel,
            SyntaxNode root,
            List<object> nodes,
            List<object> edges
        );
    }
}
