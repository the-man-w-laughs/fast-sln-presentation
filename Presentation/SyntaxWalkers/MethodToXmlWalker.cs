using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Presentation.Contracts;
using Presentation.Extensions;

namespace Presentation.SyntaxWalkers
{
    public class MethodToXmlWalker : CSharpSyntaxWalker, ISourceCodeToXmlWalker
    {
        // resulting field
        private XmlElement _xmlElement;

        // inner fields
        private SemanticModel _semanticModel;
        private readonly SyntaxNode _root;
        private XmlElement _currentElement;

        public MethodToXmlWalker(
            SemanticModel semanticModel,
            SyntaxNode root,
            XmlElement xmlElement
        )
        {
            _semanticModel = semanticModel;
            _root = root;
            _xmlElement = xmlElement;
            _currentElement = xmlElement;
        }

        public void Parse()
        {
            Visit(_root);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            XmlElement methodElement = _xmlElement.AppendMethod();
            methodElement.SetName(node.Identifier.ValueText);

            _currentElement = methodElement;

            base.VisitMethodDeclaration(node);
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            XmlElement forElement = _currentElement.AppendFor();
            XmlElement initElement = forElement.AppendInitialization();

            var declarationNode = node.Declaration;

            if (declarationNode != null)
            {
                var variableType = declarationNode.Type.ToString();

                foreach (VariableDeclaratorSyntax declarator in declarationNode.Variables)
                {
                    initElement.AppendVariable().SetName($"{variableType} {declarator}");
                }
            }
            else if (node.Initializers.Count > 0)
            {
                foreach (ExpressionSyntax initializer in node.Initializers)
                {
                    initElement.AppendVariable().SetValue(initializer.ToString());
                }
            }

            if (node.Condition != null)
            {
                XmlElement conditionElement = initElement.AppendCondition();
                conditionElement.SetValue(node.Condition.ToString());
            }

            if (node.Incrementors.Count > 0)
            {
                foreach (ExpressionSyntax incrementor in node.Incrementors)
                {
                    initElement.AppendIncrementor().SetValue(incrementor.ToString());
                }
            }

            _currentElement = forElement;

            base.VisitForStatement(node);
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            XmlElement forEachElement = _currentElement!.AppendForEach();
            XmlElement initElement = forEachElement.AppendInitialization();

            var variableType = node.Type.ToString();

            initElement
                .AppendVariable()
                .SetValue($"{variableType} {node.Identifier} in {node.Expression}");

            _currentElement = forEachElement;

            base.VisitForEachStatement(node);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            XmlElement whileElement = _currentElement!.AppendWhile();
            XmlElement initElement = whileElement.AppendInitialization();

            initElement.AppendCondition().SetValue(node.Condition.ToString());

            _currentElement = whileElement;

            base.VisitWhileStatement(node);
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            XmlElement ifElement = _currentElement!.AppendIf();
            XmlElement condition = ifElement.AppendInitialization().AppendCondition();

            condition.SetValue(node.Condition.ToString());

            // Handle the "if" block
            XmlElement ifBlock = ifElement.AppendIfBlock();
            _currentElement = ifBlock;
            base.Visit(node.Statement);

            // Handle the "else" block if present
            if (node.Else != null)
            {
                XmlElement elseBlock = ifElement.AppendElseBlock();
                _currentElement = elseBlock;
                base.Visit(node.Else.Statement);
            }

            _currentElement = ifElement;
        }

        public override void VisitSwitchExpression(SwitchExpressionSyntax node)
        {
            XmlElement switchElement = _currentElement!.AppendSwitch();
            switchElement
                .AppendInitialization()
                .AppendVariable()
                .SetValue(node.GoverningExpression.ToString());

            foreach (SwitchExpressionArmSyntax arm in node.Arms)
            {
                XmlElement armElement = switchElement.AppendCase();

                // Handle patterns if present
                if (arm.Pattern != null)
                {
                    armElement.SetValue(arm.Pattern.ToString());
                }

                _currentElement = armElement;
                base.Visit(arm);
            }

            _currentElement = switchElement;

            base.VisitSwitchExpression(node);
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            foreach (VariableDeclaratorSyntax declarator in node.Declaration.Variables)
            {
                XmlElement localDeclarationElement = _currentElement!.AppendLocalDeclaration();

                var variableType = node.Declaration.Type.ToString();
                localDeclarationElement
                    .SetType(variableType)
                    .SetName(declarator.Identifier.ToString());

                if (declarator.Initializer != null)
                {
                    localDeclarationElement.SetValue(declarator.Initializer.Value.ToString());
                }
            }

            base.VisitLocalDeclarationStatement(node);
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            if (
                node.IsKind(SyntaxKind.SimpleAssignmentExpression)
                && node.Left is IdentifierNameSyntax identifier
            )
            {
                XmlElement assignmentElement = _currentElement!.AppendAssignment();

                var variableName = identifier.Identifier.ToString();
                var assignedValue = node.Right.ToString();

                assignmentElement.SetName(variableName).SetValue(assignedValue);
            }

            base.VisitAssignmentExpression(node);
        }
    }
}
