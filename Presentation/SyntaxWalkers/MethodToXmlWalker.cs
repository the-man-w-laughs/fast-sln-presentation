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
            var methodElement = _xmlElement.AppendMethod();
            methodElement.SetName(node.Identifier.ValueText);

            _currentElement = methodElement;

            base.VisitMethodDeclaration(node);
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            var forElement = _currentElement.AppendFor();
            var initElement = forElement.AppendInitialization();

            var declarationNode = node.Declaration;

            if (declarationNode != null)
            {
                var variableType = declarationNode.Type.ToString();

                foreach (var declarator in declarationNode.Variables)
                {
                    initElement.AppendVariable().SetName($"{variableType} {declarator}");
                }
            }
            else if (node.Initializers.Count > 0)
            {
                foreach (var initializer in node.Initializers)
                {
                    initElement.AppendVariable().SetValue(initializer.ToString());
                }
            }

            if (node.Condition != null)
            {
                var conditionElement = initElement.AppendCondition();
                conditionElement.SetValue(node.Condition.ToString());
            }

            if (node.Incrementors.Count > 0)
            {
                foreach (var incrementor in node.Incrementors)
                {
                    initElement.AppendIncrementor().SetValue(incrementor.ToString());
                }
            }
            var previousElement = _currentElement;
            _currentElement = forElement;
            base.VisitForStatement(node);
            _currentElement = previousElement;
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            var forEachElement = _currentElement!.AppendForEach();
            var initElement = forEachElement.AppendInitialization();

            var variableType = node.Type.ToString();

            initElement
                .AppendVariable()
                .SetValue($"{variableType} {node.Identifier} in {node.Expression}");

            var previousElement = _currentElement;
            _currentElement = forEachElement;
            base.VisitForEachStatement(node);
            _currentElement = previousElement;
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            var whileElement = _currentElement!.AppendWhile();
            var initElement = whileElement.AppendInitialization();

            initElement.AppendCondition().SetValue(node.Condition.ToString());

            var previousElement = _currentElement;
            _currentElement = whileElement;
            base.VisitWhileStatement(node);
            _currentElement = previousElement;
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            var ifElement = _currentElement!.AppendIf();
            var condition = ifElement.AppendInitialization().AppendCondition();

            condition.SetValue(node.Condition.ToString());

            // Handle the "if" block
            var ifBlock = ifElement.AppendIfBlock();
            var previousElement = _currentElement;
            _currentElement = ifBlock;
            base.Visit(node.Statement);

            // Handle the "else" block if present
            if (node.Else != null)
            {
                var elseBlock = ifElement.AppendElseBlock();
                _currentElement = elseBlock;
                base.Visit(node.Else.Statement);
            }

            _currentElement = previousElement;
        }

        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            var switchElement = _currentElement!.AppendSwitch();

            if (node.Expression != null)
            {
                switchElement
                    .AppendInitialization()
                    .AppendVariable()
                    .SetValue(node.Expression.ToString());
            }

            var previousElement = _currentElement;

            foreach (var section in node.Sections)
            {
                foreach (var label in section.Labels)
                {
                    switch (label)
                    {
                        case CaseSwitchLabelSyntax caseLabel:
                            var caseElement = switchElement.AppendCase();
                            caseElement.SetValue(caseLabel.Value.ToString());
                            _currentElement = caseElement;
                            base.VisitCaseSwitchLabel(caseLabel);
                            break;

                        case CasePatternSwitchLabelSyntax patternLabel:
                            var patternElement = switchElement.AppendCase();
                            patternElement.SetValue(patternLabel.Pattern.ToString());
                            _currentElement = patternElement;
                            base.VisitCasePatternSwitchLabel(patternLabel);
                            break;

                        case DefaultSwitchLabelSyntax defaultLabel:
                            var defaultElement = switchElement.AppendCase();
                            _currentElement = defaultElement;
                            base.VisitDefaultSwitchLabel(defaultLabel);
                            break;
                    }
                }

                base.Visit(section);
            }

            _currentElement = previousElement;
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            foreach (var declarator in node.Declaration.Variables)
            {
                var localDeclarationElement = _currentElement!.AppendLocalDeclaration();

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
            if (node.Left is IdentifierNameSyntax identifier)
            {
                var assignmentElement = _currentElement!.AppendAssignment();

                var variableName = identifier.Identifier.ToString();
                var assignedValue = node.Right.ToString();
                var operatorString = node.OperatorToken.Text;

                assignmentElement
                    .SetName(variableName)
                    .SetValue(assignedValue)
                    .SetOperator(operatorString);
            }

            base.VisitAssignmentExpression(node);
        }

        public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            var operand = node.Operand.ToString();
            var operatorString = node.OperatorToken.Text;
            _currentElement.AppendPrefixExpression().SetOperator(operatorString).SetName(operand);
            base.VisitPrefixUnaryExpression(node);
        }

        public override void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            var operand = node.Operand.ToString();
            var operatorString = node.OperatorToken.Text;
            _currentElement.AppendPostfixExpression().SetOperator(operatorString).SetName(operand);
            base.VisitPostfixUnaryExpression(node);
        }
    }
}
