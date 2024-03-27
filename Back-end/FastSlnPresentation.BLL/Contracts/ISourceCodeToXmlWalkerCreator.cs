using Microsoft.CodeAnalysis;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Models.JsonModels.Edges;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;

namespace FastSlnPresentation.BLL.Creators
{
    public interface ISourceCodeToJsonWalkerCreator
    {
        ISourceCodeToJsonWalker Create(
            SemanticModel semanticModel,
            SyntaxNode root,
            List<INode> nodes,
            List<JsonEdge> edges
        );
    }
}
