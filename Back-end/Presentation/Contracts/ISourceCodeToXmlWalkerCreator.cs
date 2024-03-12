using Microsoft.CodeAnalysis;
using Presentation.Contracts;
using Presentation.Models.JsonModels.Edges;
using Presentation.Models.JsonModels.Nodes;

namespace Presentation.Creators
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
