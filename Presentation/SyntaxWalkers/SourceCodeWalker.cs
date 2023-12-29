using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Presentation.Contracts;
using Presentation.Extensions;
using Presentation.Models;
using Presentation.Models.CodeRepresentation;

namespace Presentation.SyntaxWalkers
{
    public class SourceCodeWalker : CSharpSyntaxWalker, ISourceCodeWalker
    {
        // resulting field
        private XmlElement _xmlElement;

        // services

        // inner fields
        private SemanticModel _semanticModel;
        private readonly SyntaxNode _root;
        private int _nestingDepth = 0;
        private readonly List<RelationModel> _relations = new();
        private readonly List<SyntaxNode> _innerTypes = new();

        private XmlElement? _currentTypeElement;

        public SourceCodeWalker(SemanticModel semanticModel, SyntaxNode root, XmlElement xmlElement)
        {
            _semanticModel = semanticModel;
            _root = root;
            _xmlElement = xmlElement;
        }

        public void Parse()
        {
            Visit(_root);
            // VisitInternalTypes();
            // AppendRelationships();
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            var symbol = _semanticModel.GetDeclaredSymbol(node);

            if (symbol == null)
            {
                throw new InvalidOperationException("Symbol information is not available.");
            }

            // Extract info
            var className = symbol.Name;
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();
            var genericParamsStr = symbol.TypeParameters.Any()
                ? string.Join(", ", symbol.TypeParameters.Select(param => param.Name))
                : "";
            var modifiersStr = string.Join(
                " ",
                node.Modifiers.Select(modifier => modifier.ToString())
            );
            AddInheritanceFrom(node);

            // Write info into xml
            var classElement = _xmlElement.CreateClass();
            classElement.SetName(className);
            classElement.SetNamespace(namespaceName);
            classElement.SetGenericParameters(genericParamsStr);
            classElement.SetModifiers(modifiersStr);

            _nestingDepth++;
            _currentTypeElement = classElement;
            base.VisitClassDeclaration(node);
            _nestingDepth--;
        }

        public void AddInheritanceFrom(TypeDeclarationSyntax type)
        {
            if (type.BaseList == null)
                return;

            var symbol = _semanticModel.GetDeclaredSymbol(type);

            if (symbol == null)
                throw new InvalidOperationException("Failed to get symbol information.");

            var sourceTypeName = symbol.ToDisplayString();

            foreach (var baseTypeSyntax in type.BaseList.Types)
            {
                var baseTypeInfo = _semanticModel.GetTypeInfo(baseTypeSyntax.Type);
                var baseType = baseTypeInfo.Type;

                if (baseType == null)
                    throw new InvalidOperationException(
                        "Failed to get type information for the base type."
                    );

                var relationType =
                    baseType.TypeKind == TypeKind.Interface
                        ? RelationType.Implementation
                        : RelationType.Inheritance;

                var targetTypeName = baseType.ToDisplayString();
                _relations.Add(new RelationModel(sourceTypeName, targetTypeName, relationType));
            }
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            if (_currentTypeElement != null)
            {
                var fieldSymbol = _semanticModel.GetDeclaredSymbol(
                    node.Declaration.Variables.First()
                );

                // Check if the field symbol is available
                if (fieldSymbol != null)
                {
                    var fieldName = fieldSymbol.Name;
                    var fieldType = _semanticModel.GetSymbolInfo(node.Declaration.Type).Symbol;

                    // Write field information into the current class element
                    var fieldElement = _currentTypeElement.CreateField();
                    fieldElement.SetName(fieldName);
                    // fieldElement.SetType(fieldType.ToDisplayString());
                }
            }

            base.VisitFieldDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            _nestingDepth++;
            base.VisitInterfaceDeclaration(node);
            _nestingDepth--;
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            _nestingDepth++;
            base.VisitStructDeclaration(node);
            _nestingDepth--;
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            _nestingDepth++;
            base.VisitEnumDeclaration(node);
            _nestingDepth--;
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            _nestingDepth++;
            base.VisitRecordDeclaration(node);
            _nestingDepth--;
        }

        private bool SkipInnerTypeDeclaration(SyntaxNode node)
        {
            if (_nestingDepth > 0)
                return true;

            _innerTypes.Add(node);

            return false;
        }

        private void AppendRelationships()
        {
            throw new NotImplementedException();
        }

        private void VisitInternalTypes()
        {
            foreach (var innerType in _innerTypes)
            {
                Visit(innerType);
            }
        }
    }
}
