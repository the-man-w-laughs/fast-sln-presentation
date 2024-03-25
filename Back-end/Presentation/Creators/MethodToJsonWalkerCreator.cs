using Microsoft.CodeAnalysis;
using Presentation.Contracts;
using Presentation.Models.JsonModels.Edges;
using Presentation.Models.JsonModels.Nodes;
using Presentation.Services;
using Presentation.SyntaxWalkers;

namespace Presentation.Creators
{
    public class MethodToJsonWalkerCreator : ISourceCodeToJsonWalkerCreator
    {
        private IdService _idService;

        public MethodToJsonWalkerCreator(IdService idService)
        {
            _idService = idService;
        }

        public ISourceCodeToJsonWalker Create(
            SemanticModel semanticModel,
            SyntaxNode root,
            List<INode> nodes,
            List<JsonEdge> edges
        )
        {
            return new MethodToJsonWalker(nodes, edges, semanticModel, root, _idService);
        }
    }
}
