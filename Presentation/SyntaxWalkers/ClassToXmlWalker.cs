using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Presentation.Contracts;
using Presentation.Extensions;

namespace Presentation.SyntaxWalkers
{
    public class ClassToXmlWalker : CSharpSyntaxWalker, ISourceCodeToXmlWalker
    {
        // resulting field
        private XmlElement _xmlElement;

        // services

        // inner fields
        private SemanticModel _semanticModel;
        private readonly SyntaxNode _root;
        private int _nestingDepth = 0;
        private readonly List<SyntaxNode> _innerTypes = new();

        private XmlElement? _currentTypeElement;

        public ClassToXmlWalker(SemanticModel semanticModel, SyntaxNode root, XmlElement xmlElement)
        {
            _semanticModel = semanticModel;
            _root = root;
            _xmlElement = xmlElement;
        }

        public void Parse()
        {
            Visit(_root);
            VisitInternalTypes();
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

            // Write info into xml
            var classElement = _xmlElement.AppendClass();
            classElement.SetName(className);
            classElement.SetNamespace(namespaceName);
            classElement.SetModifiers(modifiersStr);
            classElement.SetGenericParameters(genericParamsStr);

            var parentClass = node.Parent as ClassDeclarationSyntax;
            if (parentClass != null)
            {
                var parentSymbol = _semanticModel.GetDeclaredSymbol(parentClass);
                if (parentSymbol != null)
                {
                    var parentClassName = parentSymbol.ToDisplayString();
                    classElement.SetParentClass(parentClassName);
                }
            }

            _nestingDepth++;
            _currentTypeElement = classElement;
            AddInheritanceFrom(node);
            base.VisitClassDeclaration(node);
            _nestingDepth--;
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
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

            // Write information into XML or other data structure
            var interfaceElement = _xmlElement!.AppendInterface();
            interfaceElement.SetName(className);
            interfaceElement.SetNamespace(namespaceName);
            interfaceElement.SetModifiers(modifiersStr);
            interfaceElement.SetGenericParameters(genericParamsStr);

            _nestingDepth++;
            _currentTypeElement = interfaceElement;
            AddInheritanceFrom(node);
            base.VisitInterfaceDeclaration(node);
            _nestingDepth--;
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            // Extract info
            var structName = symbol.Name;
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();
            var genericParamsStr = symbol.TypeParameters.Any()
                ? string.Join(", ", symbol.TypeParameters.Select(param => param.Name))
                : "";
            var modifiersStr = string.Join(
                " ",
                node.Modifiers.Select(modifier => modifier.ToString())
            );

            // Write information into XML or other data structure
            var structElement = _xmlElement!.AppendStruct();
            structElement.SetName(structName);
            structElement.SetNamespace(namespaceName);
            structElement.SetModifiers(modifiersStr);
            structElement.SetGenericParameters(genericParamsStr);

            _nestingDepth++;
            _currentTypeElement = structElement;
            AddInheritanceFrom(node);
            base.VisitStructDeclaration(node);
            _nestingDepth--;
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            var enumName = symbol.Name;
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();
            var underlyingType = symbol.EnumUnderlyingType?.ToDisplayString() ?? "int";
            var modifiersStr = string.Join(
                " ",
                node.Modifiers.Select(modifier => modifier.ToString())
            );

            var enumElement = _xmlElement!.AppendEnum();
            enumElement.SetName(enumName);
            enumElement.SetNamespace(namespaceName);
            enumElement.SetModifiers(modifiersStr);
            enumElement.SetUnderlyingType(underlyingType);

            foreach (var member in node.Members)
            {
                var memberName = member.Identifier.Text;
                var memberValue = member.EqualsValue?.Value?.ToString() ?? "0";

                var enumMemberElement = enumElement.AppendMember();
                enumMemberElement.SetName(memberName);
                enumMemberElement.SetValue(memberValue);
            }

            _nestingDepth++;
            _currentTypeElement = enumElement;
            base.VisitEnumDeclaration(node);
            _nestingDepth--;
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            // Extract info
            var recordName = symbol.Name;
            var namespaceName = symbol.ContainingNamespace.ToDisplayString();
            var modifiersStr = string.Join(
                " ",
                node.Modifiers.Select(modifier => modifier.ToString())
            );

            // Write information into XML or other data structure
            var recordElement = _xmlElement!.AppendRecord();
            recordElement.SetName(recordName);
            recordElement.SetNamespace(namespaceName);
            recordElement.SetModifiers(modifiersStr);

            _nestingDepth++;
            _currentTypeElement = recordElement;
            AddInheritanceFrom(node);
            base.VisitRecordDeclaration(node);
            _nestingDepth--;
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(node);

            if (symbol == null)
                return;

            // Extract information from the method declaration
            var methodName = symbol.Name;
            var returnType = symbol.ReturnType?.ToDisplayString() ?? "void";
            var modifiersStr = string.Join(
                " ",
                node.Modifiers.Select(modifier => modifier.ToString())
            );

            // Write information into XML or other data structure
            var methodElement = _currentTypeElement!.AppendMethod();
            methodElement.SetName(methodName);
            methodElement.SetReturnType(returnType);
            methodElement.SetModifiers(modifiersStr);

            // Handle method parameters
            var parameterList = node.ParameterList;
            if (parameterList != null)
            {
                foreach (var parameter in parameterList.Parameters)
                {
                    var paramName = parameter.Identifier.Text;
                    var paramType =
                        _semanticModel.GetTypeInfo(parameter.Type!).Type?.ToDisplayString()
                        ?? "unknown";

                    // Create and append the Parameter element
                    var parameterElement = methodElement.AppendParameter()!;
                    parameterElement.SetName(paramName);
                    parameterElement.SetType(paramType);
                    methodElement.AppendChild(parameterElement);
                }
            }

            base.VisitMethodDeclaration(node);
        }

        public void AddInheritanceFrom(TypeDeclarationSyntax type)
        {
            if (type.BaseList == null)
                return;

            var baseTypesElement = _currentTypeElement!.AppendBaseTypes();

            foreach (var baseTypeSyntax in type.BaseList.Types)
            {
                var baseTypeInfo = _semanticModel.GetTypeInfo(baseTypeSyntax.Type);
                var baseType = baseTypeInfo.Type as INamedTypeSymbol;

                if (baseType == null)
                    throw new InvalidOperationException(
                        "Failed to get type information for the base type."
                    );

                var targetTypeName = baseType.ToDisplayString();

                var typeElement = baseTypesElement.AppendType();
                typeElement.SetName(targetTypeName);

                baseTypesElement.AppendChild(typeElement);
            }
            _currentTypeElement?.AppendChild(baseTypesElement);
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            var fieldTypeSyntax = node.Declaration.Type;
            var isNullable = fieldTypeSyntax is NullableTypeSyntax;
            var isPredefinedType = fieldTypeSyntax is PredefinedTypeSyntax;

            var fieldModifiers = string.Join(
                " ",
                node.Modifiers.Select(modifier => modifier.ToString())
            );

            // Iterate over each variable in the field declaration
            foreach (var variable in node.Declaration.Variables)
            {
                var fieldName = variable.Identifier.Text;
                var fieldType = _semanticModel.GetTypeInfo(fieldTypeSyntax).Type!;
                var initializationValue = variable.Initializer?.Value.ToString() ?? string.Empty;

                var fieldElement = _currentTypeElement!.AppendField();
                fieldElement.SetName(fieldName);
                fieldElement.SetType(fieldType.ToDisplayString());

                fieldElement.SetIsNullable(isNullable);
                fieldElement.SetIsPredefinedType(isPredefinedType);
                fieldElement.SetIsReferenceType(fieldType.IsReferenceType);
                fieldElement.SetValue(initializationValue);
                fieldElement.SetModifiers(fieldModifiers);
            }

            base.VisitFieldDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var propertyTypeSyntax = node.Type;
            var isNullable = propertyTypeSyntax is NullableTypeSyntax;
            var isPredefinedType = propertyTypeSyntax is PredefinedTypeSyntax;

            var propertyModifiers = string.Join(
                " ",
                node.Modifiers.Select(modifier => modifier.ToString())
            );
            var propertyName = node.Identifier.Text;
            var propertyType = _semanticModel.GetTypeInfo(node.Type).Type!;
            var accessorList = node.AccessorList;

            var propertyElement = _currentTypeElement!.AppendProperty();
            propertyElement.SetName(propertyName);
            propertyElement.SetType(propertyType.ToDisplayString());
            propertyElement.SetIsNullable(node.Type is NullableTypeSyntax);
            propertyElement.SetIsPredefinedType(node.Type is PredefinedTypeSyntax);
            propertyElement.SetIsReferenceType(propertyType.IsReferenceType);
            propertyElement.SetModifiers(propertyModifiers);

            var hasGetter =
                node.ExpressionBody != null
                || (
                    accessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration))
                    ?? false
                );
            var hasSetter =
                accessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration))
                ?? false;

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

            base.VisitPropertyDeclaration(node);
        }

        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            var modifiers = string.Join(
                " ",
                node.Modifiers.Select(modifier => modifier.ToString())
            );
            var eventType = _semanticModel.GetTypeInfo(node.Declaration.Type).Type!;

            foreach (var variable in node.Declaration.Variables)
            {
                var eventName = variable.Identifier.Text;

                var eventElement = _currentTypeElement!.AppendEvent();
                eventElement.SetName(eventName);
                eventElement.SetType(eventType.ToDisplayString());
                eventElement.SetIsNullable(node.Declaration.Type is NullableTypeSyntax);
                eventElement.SetIsPredefinedType(node.Declaration.Type is PredefinedTypeSyntax);
                eventElement.SetIsReferenceType(eventType.IsReferenceType);
                eventElement.SetModifiers(modifiers);
            }

            base.VisitEventFieldDeclaration(node);
        }

        private bool SkipInnerTypeDeclaration(SyntaxNode node)
        {
            if (_nestingDepth == 0)
            {
                return false;
            }

            _innerTypes.Add(node);
            return true;
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
