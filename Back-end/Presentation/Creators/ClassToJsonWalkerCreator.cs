using System.Xml;
using Microsoft.CodeAnalysis;
using Presentation.Contracts;
using Presentation.Services;
using Presentation.SyntaxWalkers;

namespace Presentation.Creators;

public class ClassToJsonWalkerCreator : ISourceCodeToXmlWalkerCreator
{
    private readonly IIdSerivice _idSerivice;
    private readonly IModifiersMappingHelper _modifiersMappingHelper;

    public ClassToJsonWalkerCreator(
        IIdSerivice idSerivice,
        IModifiersMappingHelper modifiersMappingHelper
    )
    {
        _idSerivice = idSerivice;
        _modifiersMappingHelper = modifiersMappingHelper;
    }

    public ISourceCodeToJsonWalker Create(
        SemanticModel semanticModel,
        SyntaxNode root,
        List<object> nodes,
        List<object> edges
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
