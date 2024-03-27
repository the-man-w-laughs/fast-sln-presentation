using Microsoft.CodeAnalysis;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Models.JsonModels.Edges;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using FastSlnPresentation.BLL.SyntaxWalkers;

namespace FastSlnPresentation.BLL.Creators;

public class SourceToJsonWalkerCreator : ISourceCodeToJsonWalkerCreator
{
    private readonly IIdService _idSerivice;

    public SourceToJsonWalkerCreator(IIdService idSerivice)
    {
        _idSerivice = idSerivice;
    }

    public ISourceCodeToJsonWalker Create(
        SemanticModel semanticModel,
        SyntaxNode root,
        List<INode> nodes,
        List<JsonEdge> edges
    )
    {
        return new SourceToJsonWalker(nodes, edges, semanticModel, root, _idSerivice);
    }
}
