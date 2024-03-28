using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FastSlnPresentation.BLL.Consts;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Models.JsonModels;
using FastSlnPresentation.BLL.Models.JsonModels.Edges;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes.Data;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes.Flowchart;

namespace FastSlnPresentation.BLL.SyntaxWalkers
{
    public class MethodToJsonWalker : CSharpSyntaxWalker, IMethodToJsonWalker
    {
        // resulting field
        private readonly List<INode> _resultNodes;
        private readonly List<JsonEdge> _resultEdges;

        // inner fields
        private readonly SyntaxNode _root;
        private readonly IIdService _idSerivice;
        private List<string> _label = new();
        private List<string> _lastElementIds = new();

        public MethodToJsonWalker(
            List<INode> nodes,
            List<JsonEdge> edges,
            SyntaxNode root,
            IIdService idSerivice
        )
        {
            _resultEdges = edges;
            _resultNodes = nodes;
            _root = root;
            _idSerivice = idSerivice;
        }

        public void Parse()
        {
            Visit(_root);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var name = node.Identifier.ValueText;

            var content = $"Начало {name}";
            AddNode<JsonTerminal>(content);

            base.VisitMethodDeclaration(node);

            content = $"Конец {name}";
            AddNode<JsonTerminal>(content);
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            var declarationNode = node.Declaration;

            // add cycle variables nodes
            if (declarationNode != null)
            {
                var variableType = declarationNode.Type.ToString();

                foreach (var declarator in declarationNode.Variables)
                {
                    var content = $"{variableType} {declarator}";
                    AddNode<JsonBlock>(content);
                }
            }
            else if (node.Initializers.Count > 0)
            {
                foreach (var initializer in node.Initializers)
                {
                    var content = initializer.ToString();
                    AddNode<JsonBlock>(content);
                }
            }

            // add cycle node
            var cycleId = _idSerivice.GetNextId("Cycle");
            var cycleContent = new List<string>()
            {
                cycleId,
                node.Condition?.ToString() ?? string.Empty
            };
            AddNode<JsonOpenCycle>(cycleContent);

            base.Visit(node.Statement);

            if (node.Incrementors.Count > 0)
            {
                foreach (var incrementor in node.Incrementors)
                {
                    var content = incrementor.ToString();
                    AddNode<JsonBlock>(content);
                }
            }

            AddNode<JsonCloseCycle>(cycleId);
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            var variableType = node.Type.ToString();

            var enumeratorIdentifier = _idSerivice.GetNextId("Enumerator", "{0}{1}");
            var content = $"var {enumeratorIdentifier} = {node.Expression}.GetEnumerator();";
            AddNode<JsonBlock>(content);

            var cycleId = _idSerivice.GetNextId("Cycle");
            var cycleCondition = $"{enumeratorIdentifier}.MoveNext()";
            var cycleContent = new List<string>() { cycleId, cycleCondition };
            AddNode<JsonOpenCycle>(cycleContent);

            var nodeIdentifier = node.Identifier;
            content = $"{variableType} {nodeIdentifier} = {enumeratorIdentifier}.Current;";
            AddNode<JsonBlock>(content);

            base.Visit(node.Statement);
            AddNode<JsonCloseCycle>(cycleId);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            var cycleId = _idSerivice.GetNextId("Cycle");
            var cycleCondition = node.Condition.ToString();
            var cycleContent = new List<string>() { cycleId, cycleCondition };
            AddNode<JsonOpenCycle>(cycleContent);
            base.Visit(node.Statement);
            AddNode<JsonCloseCycle>(cycleId);
        }

        public override void VisitDoStatement(DoStatementSyntax node)
        {
            var cycleId = _idSerivice.GetNextId("Cycle");
            AddNode<JsonCloseCycle>(cycleId);
            base.Visit(node.Statement);
            var cycleCondition = node.Condition.ToString();
            var cycleContent = new List<string>() { cycleId, cycleCondition };
            AddNode<JsonOpenCycle>(cycleContent);
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            var condition = node.Condition.ToString();

            // Handle the "if" block
            AddNode<JsonCondition>(condition);
            var conditionId = _lastElementIds.ToList();
            _label.Add("Да");
            base.Visit(node.Statement);
            var lastNodes = _lastElementIds.ToList();
            _lastElementIds = conditionId;
            // Handle the "else" block if present
            if (node.Else != null)
            {
                _label.Add("Нет");
                base.Visit(node.Else.Statement);
                _lastElementIds.AddRange(lastNodes);
            }
        }

        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            var expression = node.Expression.ToString();
            AddNode<JsonCondition>(expression);
            var conditionId = _lastElementIds.ToList();
            var lastNodes = new List<string>();
            foreach (var section in node.Sections)
            {
                foreach (var label in section.Labels)
                {
                    switch (label)
                    {
                        case CaseSwitchLabelSyntax caseLabel:
                            var caseLabelStr = caseLabel.Value.ToString();
                            _label.Add(caseLabelStr);
                            break;

                        case CasePatternSwitchLabelSyntax patternLabel:
                            var patternLabelStr = patternLabel.Pattern.ToString();
                            _label.Add(patternLabelStr);
                            break;

                        case DefaultSwitchLabelSyntax defaultLabel:
                            var defaultLabelStr = "default";
                            _label.Add(defaultLabelStr);
                            break;
                    }
                }
                _lastElementIds = conditionId.ToList();
                base.VisitSwitchSection(section);
                lastNodes.AddRange(_lastElementIds);
            }
            _lastElementIds = lastNodes.ToList();
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            foreach (var declarator in node.Declaration.Variables)
            {
                var variableType = node.Declaration.Type.ToString();
                var name = declarator.Identifier.ToString();
                var initializer = declarator.Initializer?.Value.ToString() ?? "";

                var content =
                    $"{variableType} {name}{(initializer != "" ? " = " + initializer : "")};";
                AddNode<JsonBlock>(content);
            }

            base.VisitLocalDeclarationStatement(node);
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            if (node.Left is IdentifierNameSyntax identifier)
            {
                var variableName = identifier.Identifier.ToString();
                var assignedValue = node.Right.ToString();
                var operatorString = node.OperatorToken.Text;

                var content = $"{variableName} {operatorString} {assignedValue};";
                AddNode<JsonBlock>(content);
            }

            base.VisitAssignmentExpression(node);
        }

        public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            var operand = node.Operand.ToString();
            var operatorString = node.OperatorToken.Text;

            var content = $"{operatorString}{operand};";
            AddNode<JsonBlock>(content);
            base.VisitPrefixUnaryExpression(node);
        }

        public override void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            var operand = node.Operand.ToString();
            var operatorString = node.OperatorToken.Text;

            var content = $"{operatorString}{operand};";
            AddNode<JsonBlock>(content);
            base.VisitPostfixUnaryExpression(node);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            string containingTypeName = "";

            string methodName;
            if (node.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                methodName = memberAccess.Name.ToString();
                containingTypeName = memberAccess.Expression.ToString();
            }
            else
            {
                methodName = node.Expression.ToString();
            }

            string arguments = string.Join(", ", node.ArgumentList.Arguments);

            string invocationString =
                $"{containingTypeName}{(string.IsNullOrEmpty(containingTypeName) ? "" : ".")}{methodName}({arguments})";

            AddNode<JsonBlock>(invocationString);

            base.VisitInvocationExpression(node);
        }

        private void AddNode<Node>(string content)
            where Node : INode, IFlowchartNode, new()
        {
            AddNode<Node>(new List<string>() { content });
        }

        private void AddNode<Node>(List<string> content)
            where Node : INode, IFlowchartNode, new()
        {
            var node = new Node() { Data = new BlockData(content) };
            var id = _idSerivice.GetNextId(node.Type, "{0}{1}");
            node.Id = id;
            AddAndConnectNode(node);
        }

        private void AddAndConnectNode(INode node)
        {
            foreach (var lastElementId in _lastElementIds)
            {
                AddConnection(lastElementId, node.Id);
            }
            AddNode(node);
        }

        private void AddNode(INode node)
        {
            _resultNodes.Add(node);
            _lastElementIds.Clear();
            _lastElementIds.Add(node.Id);
        }

        private void AddConnection(string sourceNodeId, string targetNodeId)
        {
            // var id = _idSerivice.GetNextId();
            var id = $"{sourceNodeId}-{targetNodeId}";
            var edge = new JsonEdge(
                id,
                targetNodeId,
                sourceNodeId,
                EdgeTypes.Arrow,
                _label.ToList()
            );
            _resultEdges.Add(edge);
            _label.Clear();
        }
    }
}
