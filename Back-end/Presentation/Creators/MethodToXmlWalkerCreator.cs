using System.Xml;
using Microsoft.CodeAnalysis;
using Presentation.Contracts;
using Presentation.SyntaxWalkers;

namespace Presentation.Creators
{
    public class MethodToXmlWalkerCreator : ISourceCodeToXmlWalkerCreator
    {
        public ISourceCodeToXmlWalker Create(
            SemanticModel semanticModel,
            SyntaxNode root,
            XmlElement xmlElement
        )
        {
            return new MethodToXmlWalker(semanticModel, root, xmlElement);
        }
    }
}
