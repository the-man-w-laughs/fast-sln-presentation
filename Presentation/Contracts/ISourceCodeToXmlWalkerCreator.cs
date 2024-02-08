using System.Xml;
using Microsoft.CodeAnalysis;
using Presentation.Contracts;

namespace Presentation.Creators
{
    public interface ISourceCodeToXmlWalkerCreator
    {
        ISourceCodeToXmlWalker Create(
            SemanticModel semanticModel,
            SyntaxNode root,
            XmlElement xmlElement
        );
    }
}
