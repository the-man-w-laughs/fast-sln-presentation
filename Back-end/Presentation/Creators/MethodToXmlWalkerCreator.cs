// using System.Xml;
// using Microsoft.CodeAnalysis;
// using Presentation.Contracts;
// using Presentation.SyntaxWalkers;

// namespace Presentation.Creators
// {
//     public class MethodToXmlWalkerCreator : ISourceCodeToXmlWalkerCreator
//     {
//         public ISourceCodeToXmlWalker Create(
//             SemanticModel semanticModel,
//             SyntaxNode root,
//             XmlElement xmlElement
//         )
//         {
//             return new MethodToXmlWalker(semanticModel, root, xmlElement);
//         }

//         public ISourceCodeToXmlWalker Create(
//             SemanticModel semanticModel,
//             SyntaxNode root,
//             List<object> nodes,
//             List<object> edges
//         )
//         {
//             var xmlElement = new XmlElement();
//             return new MethodToXmlWalker(semanticModel, root, xmlElement);
//         }
//     }
// }
