using Microsoft.CodeAnalysis;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Models.JsonModels.Edges;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using FastSlnPresentation.BLL.Services;
using FastSlnPresentation.BLL.SyntaxWalkers;

namespace FastSlnPresentation.BLL.Creators
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
