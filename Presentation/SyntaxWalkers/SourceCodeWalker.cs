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

            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

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
            var classElement = _xmlElement.AppendClass();
            classElement.SetName(className);
            classElement.SetNamespace(namespaceName);
            classElement.SetModifiers(modifiersStr);

            if (genericParamsStr != string.Empty)
            {
                classElement.SetGenericParameters(genericParamsStr);
            }

            _nestingDepth++;
            _currentTypeElement = classElement;
            base.VisitClassDeclaration(node);
            _nestingDepth--;
        }

        public void AddInheritanceFrom(TypeDeclarationSyntax type)
        {
            if (type.BaseList == null)
                return;

            var symbol = _semanticModel.GetDeclaredSymbol(type)!;

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
            var fieldTypeSyntax = node.Declaration.Type;
            var isNullable = fieldTypeSyntax is NullableTypeSyntax;
            var isPredefinedType = fieldTypeSyntax is PredefinedTypeSyntax;

            if (isNullable || isPredefinedType)
            {
                var fieldModifiers = string.Join(
                    ", ",
                    node.Modifiers.Select(modifier => modifier.ToString())
                );

                // Iterate over each variable in the field declaration
                foreach (var variable in node.Declaration.Variables)
                {
                    var fieldName = variable.Identifier.Text;
                    var fieldType = _semanticModel.GetTypeInfo(fieldTypeSyntax).Type!;
                    var initializationValue =
                        variable.Initializer?.Value.ToString() ?? string.Empty;

                    var fieldElement = _currentTypeElement!.AppendField();
                    fieldElement.SetName(fieldName);
                    fieldElement.SetType(fieldType.ToDisplayString());
                    fieldElement.SetModifiers(fieldModifiers);
                    fieldElement.SetIsNullable(isNullable);
                    fieldElement.SetIsPredefinedType(isPredefinedType);

                    if (initializationValue != string.Empty)
                    {
                        fieldElement.SetValue(initializationValue);
                    }
                }
            }

            base.VisitFieldDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var propertyTypeSyntax = node.Type;
            var isNullable = propertyTypeSyntax is NullableTypeSyntax;
            var isPredefinedType = propertyTypeSyntax is PredefinedTypeSyntax;

            if (isNullable || isPredefinedType)
            {
                var propertyModifiers = string.Join(
                    ", ",
                    node.Modifiers.Select(modifier => modifier.ToString())
                );
                var propertyName = node.Identifier.Text;
                var propertyType = _semanticModel.GetTypeInfo(node.Type).Type!;
                var accessorList = node.AccessorList;

                var hasGetter =
                    node.ExpressionBody != null
                    || (
                        accessorList?.Accessors.Any(
                            a => a.IsKind(SyntaxKind.GetAccessorDeclaration)
                        ) ?? false
                    );
                var hasSetter =
                    accessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration))
                    ?? false;

                var propertyElement = _currentTypeElement!.AppendProperty();
                propertyElement.SetName(propertyName);
                propertyElement.SetType(propertyType.ToDisplayString());
                propertyElement.SetModifiers(propertyModifiers);
                propertyElement.SetIsNullable(node.Type is NullableTypeSyntax);
                propertyElement.SetIsPredefinedType(node.Type is PredefinedTypeSyntax);

                if (hasGetter)
                {
                    var getterModifiers =
                        accessorList?.Accessors
                            .FirstOrDefault(a => a.IsKind(SyntaxKind.GetAccessorDeclaration))
                            ?.Modifiers.ToString() ?? string.Empty;
                    propertyElement.AppendGetter().SetModifiers(getterModifiers);
                }

                if (hasSetter)
                {
                    var setterModifiers =
                        accessorList?.Accessors
                            .FirstOrDefault(a => a.IsKind(SyntaxKind.SetAccessorDeclaration))
                            ?.Modifiers.ToString() ?? string.Empty;
                    propertyElement.AppendSetter().SetModifiers(setterModifiers);
                }
            }

            base.VisitPropertyDeclaration(node);
        }

        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            var variableDeclaration = node.Parent as VariableDeclarationSyntax;
            var fieldDeclaration = node.Parent as FieldDeclarationSyntax;

            if (variableDeclaration != null)
            {
                Console.WriteLine($"Composition found: {variableDeclaration.Type}");
            }
            else if (fieldDeclaration != null)
            {
                Console.WriteLine($"Composition found: {fieldDeclaration.Declaration.Type}");
            }

            base.VisitObjectCreationExpression(node);
        }

        public override void VisitEventDeclaration(EventDeclarationSyntax node)
        {
            base.VisitEventDeclaration(node);
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
