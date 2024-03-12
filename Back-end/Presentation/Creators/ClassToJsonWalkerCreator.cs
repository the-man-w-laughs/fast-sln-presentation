using Microsoft.CodeAnalysis;
using Presentation.Contracts;
using Presentation.Models.JsonModels.Edges;
using Presentation.Models.JsonModels.Nodes;
using Presentation.SyntaxWalkers;

namespace Presentation.Creators;

public class SourceToJsonWalkerCreator : ISourceCodeToJsonWalkerCreator
{
    private readonly IIdService _idSerivice;
    private readonly IModifiersMappingHelper _modifiersMappingHelper;

    public SourceToJsonWalkerCreator(
        IIdService idSerivice,
        IModifiersMappingHelper modifiersMappingHelper
    )
    {
        _idSerivice = idSerivice;
        _modifiersMappingHelper = modifiersMappingHelper;
    }

    public ISourceCodeToJsonWalker Create(
        SemanticModel semanticModel,
        SyntaxNode root,
        List<INode> nodes,
        List<JsonEdge> edges
    )
    {
        return new SourceToJsonWalker(
            nodes,
            edges,
            semanticModel,
            root,
            _idSerivice,
            _modifiersMappingHelper
        );
    }
}
