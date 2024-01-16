using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Presentation.Extensions;

namespace Presentation.SyntaxWalkers
{
    public class MethodToXmlWalker : CSharpSyntaxWalker
    {
        // resulting field
        private XmlElement _xmlElement;

        // inner fields
        private SemanticModel _semanticModel;
        private readonly SyntaxNode _root;

        public MethodToXmlWalker(
            SemanticModel semanticModel,
            SyntaxNode root,
            XmlElement xmlElement
        )
        {
            _semanticModel = semanticModel;
            _root = root;
            _xmlElement = xmlElement;
        }

        public void Parse()
        {
            Visit(_root);
        }

        public override void VisitForStatement(ForStatementSyntax node)
        {
            XmlElement forElement = _xmlElement.AppendFor();

            if (node.Declaration != null)
            {
                XmlElement initElement = forElement.AppendElement("Initialization");
                foreach (VariableDeclaratorSyntax declarator in node.Declaration.Variables)
                {
                    initElement.AppendElement("Variable").SetName(declarator.Identifier.ValueText);
                }
            }
            else if (node.Initializers.Count > 0)
            {
                XmlElement initElement = forElement.AppendElement("Initialization");
                foreach (ExpressionSyntax initializer in node.Initializers)
                {
                    initElement.AppendElement("Initializer").SetValue(initializer.ToString());
                }
            }

            if (node.Condition != null)
            {
                XmlElement conditionElement = forElement.AppendElement("Condition");
                conditionElement.SetValue(node.Condition.ToString());
            }

            if (node.Incrementors.Count > 0)
            {
                XmlElement incrementorsElement = forElement.AppendElement("Incrementors");
                foreach (ExpressionSyntax incrementor in node.Incrementors)
                {
                    incrementorsElement
                        .AppendElement("Incrementor")
                        .SetValue(incrementor.ToString());
                }
            }

            base.VisitForStatement(node);
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            base.VisitForEachStatement(node);
        }

        public override void VisitWhileStatement(WhileStatementSyntax node)
        {
            base.VisitWhileStatement(node);
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            base.VisitIfStatement(node);
        }

        public override void VisitSwitchExpression(SwitchExpressionSyntax node)
        {
            base.VisitSwitchExpression(node);
        }

        public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            base.VisitLocalDeclarationStatement(node);
        }
    }
}
