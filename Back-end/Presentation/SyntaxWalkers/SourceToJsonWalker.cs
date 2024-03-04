using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Presentation.Consts;
using Presentation.Contracts;
using Presentation.Extensions;
using Presentation.Models.JsonModels;
using Presentation.Models.JsonModels.Edges;

namespace Presentation.SyntaxWalkers
{
    public class SourceToJsonWalker : CSharpSyntaxWalker, ISourceCodeToJsonWalker
    {
        // resulting field
        private readonly List<object> _resultNodes;
        private readonly List<object> _resultEdges;

        // services
        private readonly IIdSerivice _idSerivice;
        private readonly IModifiersMappingHelper _modifiersMappingHelper;

        // inner fields
        private SemanticModel _semanticModel;
        private readonly SyntaxNode _root;
        private int _nestingDepth = 0;
        private readonly List<SyntaxNode> _innerTypes = new();
        private readonly Dictionary<string, List<string>> _innerMembers = new();
        private string? _currentTypeFullName;

        public SourceToJsonWalker(
            List<object> nodes,
            List<object> edges,
            SemanticModel semanticModel,
            SyntaxNode root,
            IIdSerivice idSerivice,
            IModifiersMappingHelper modifiersMappingHelper
        )
        {
            _resultNodes = nodes;
            _resultEdges = edges;
            _semanticModel = semanticModel;
            _root = root;
            _idSerivice = idSerivice;
            _modifiersMappingHelper = modifiersMappingHelper;
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
            var name = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var genericInfo = ExtractGenericInfo(symbol);
            var modifiersStr = ExtractModifiers(node);

            // Write info
            var jsonNode = new JsonClass(fullName, name, fullName, modifiersStr, genericInfo);

            _nestingDepth++;
            AddInheritanceFrom(node);
            _innerMembers[MemberTypes.Members] = new();
            _innerMembers[MemberTypes.Methods] = new();
            _currentTypeFullName = fullName;
            base.VisitClassDeclaration(node);
            jsonNode.Data.Members.AddRange(_innerMembers[MemberTypes.Members]);
            jsonNode.Data.Methods.AddRange(_innerMembers[MemberTypes.Methods]);
            _nestingDepth--;

            _resultNodes.Add(jsonNode);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            // Extract info
            var name = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var genericInfo = ExtractGenericInfo(symbol);
            var modifiersStr = ExtractModifiers(node);

            var jsonNode = new JsonInterface(fullName, name, fullName, modifiersStr, genericInfo);

            // Write information
            _nestingDepth++;
            AddInheritanceFrom(node);
            _innerMembers[MemberTypes.Members] = new();
            _innerMembers[MemberTypes.Methods] = new();
            _currentTypeFullName = fullName;
            base.VisitInterfaceDeclaration(node);
            jsonNode.Data.Members.AddRange(_innerMembers[MemberTypes.Members]);
            jsonNode.Data.Methods.AddRange(_innerMembers[MemberTypes.Methods]);
            _nestingDepth--;

            _resultNodes.Add(jsonNode);
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
            var fullName = symbol.ContainingNamespace.ToDisplayString();
            List<string> genericInfo = ExtractGenericInfo(symbol);
            string modifiersStr = ExtractModifiers(node);

            var jsonNode = new JsonStruct(
                fullName,
                structName,
                fullName,
                modifiersStr,
                genericInfo
            );

            // Write information into XML or other data structure

            _nestingDepth++;
            AddInheritanceFrom(node);
            _innerMembers[MemberTypes.Members] = new();
            _innerMembers[MemberTypes.Methods] = new();
            _currentTypeFullName = fullName;
            base.VisitStructDeclaration(node);
            jsonNode.Data.Members.AddRange(_innerMembers[MemberTypes.Members]);
            jsonNode.Data.Methods.AddRange(_innerMembers[MemberTypes.Methods]);
            _nestingDepth--;

            _resultNodes.Add(jsonNode);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            var enumName = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var modifiersStr = ExtractModifiers(node);
            var members = new List<string>();

            foreach (var member in node.Members)
            {
                var memberName = member.Identifier.Text;
                var memberValue = member.EqualsValue?.Value;

                members.Add($"{memberName}{(memberValue == null ? "" : $" = {memberValue}")}");
            }

            var jsonNode = new JsonEnum(fullName, enumName, fullName, modifiersStr, members);

            _nestingDepth++;
            base.VisitEnumDeclaration(node);
            _nestingDepth--;

            _resultNodes.Add(jsonNode);
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            var recordName = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var modifiersStr = ExtractModifiers(node);
            var genericInfo = ExtractGenericInfo(symbol);

            var jsonNode = new JsonRecord(
                fullName,
                recordName,
                fullName,
                modifiersStr,
                genericInfo
            );

            _nestingDepth++;

            AddInheritanceFrom(node);
            _innerMembers[MemberTypes.Members] = new();
            _innerMembers[MemberTypes.Members] = new();
            _currentTypeFullName = fullName;
            base.VisitRecordDeclaration(node);
            jsonNode.Data.Members.AddRange(_innerMembers[MemberTypes.Members]);
            jsonNode.Data.Methods.AddRange(_innerMembers[MemberTypes.Methods]);
            _nestingDepth--;

            _resultNodes.Add(jsonNode);
        }

        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            var symbol = _semanticModel.GetDeclaredSymbol(node);

            // Extract info
            var delegateName = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var modifiersStr = ExtractModifiers(node);
            var genericInfo = ExtractGenericInfo(symbol);

            var invokeMethod = symbol.DelegateInvokeMethod!;
            var returnType = invokeMethod.ReturnType.ToDisplayString();

            var parameters = new List<string>();
            foreach (var parameter in invokeMethod.Parameters)
            {
                var paramName = parameter.Name;
                var paramType = parameter.Type.ToDisplayString();
                parameters.Add($"{paramName} : {paramType}");
            }

            var jsonNode = new JsonDelegate(
                fullName,
                delegateName,
                fullName,
                modifiersStr,
                genericInfo,
                returnType,
                parameters
            );

            _nestingDepth++;
            base.VisitDelegateDeclaration(node);
            _nestingDepth--;

            _resultNodes.Add(jsonNode);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(node);

            if (symbol == null)
                return;

            // Extract information from the method declaration
            var methodName = symbol.Name;
            var returnType = symbol.ReturnType?.ToDisplayString() ?? "void";
            string modifiersStr = ExtractModifiers(node);

            // Handle method parameters
            var parametersListStr = string.Join(
                ", ",
                node.ParameterList?.Parameters.Select(parameter =>
                {
                    var paramName = parameter.Identifier.Text;
                    var paramType =
                        _semanticModel.GetTypeInfo(parameter.Type).Type?.ToDisplayString()
                        ?? "unknown";
                    return $"{paramName} : {paramType}";
                }) ?? Enumerable.Empty<string>()
            );

            var resultString = $"{modifiersStr} {methodName} ({parametersListStr}): {returnType}";
            _innerMembers[MemberTypes.Methods].Add(resultString);

            base.VisitMethodDeclaration(node);
        }

        public void AddInheritanceFrom(TypeDeclarationSyntax type)
        {
            if (type.BaseList == null)
                return;

            var symbol = _semanticModel.GetDeclaredSymbol(type)!;

            foreach (var baseTypeSyntax in type.BaseList.Types)
            {
                var baseTypeInfo = _semanticModel.GetTypeInfo(baseTypeSyntax.Type);
                var baseType = baseTypeInfo.Type as INamedTypeSymbol;

                if (baseType == null)
                    throw new InvalidOperationException(
                        "Failed to get type information for the base type."
                    );

                // skip types for other assemblies
                if (baseType.ContainingAssembly.Identity != symbol.ContainingAssembly.Identity)
                    continue;

                var edgeType =
                    baseType.TypeKind == TypeKind.Interface
                        ? EdgeTypes.Implementation
                        : EdgeTypes.Inheritance;

                var id = _idSerivice.GetNextId();
                var targetFullName = baseType.ToDisplayString();
                var sourceFullName = symbol.ToDisplayString();
                var edge = new JsonEdge(id, targetFullName, sourceFullName, edgeType);
                _resultEdges.Add(edge);
            }
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            var fieldTypeSyntax = node.Declaration.Type;
            var isNullable = fieldTypeSyntax is NullableTypeSyntax;
            var isPredefinedType = fieldTypeSyntax is PredefinedTypeSyntax;
            var fieldModifiers = ExtractModifiers(node);
            var fieldType = _semanticModel.GetTypeInfo(fieldTypeSyntax).Type!;
            var typeFullName = fieldType.ToDisplayString();
            var isReferenceType = fieldType.IsReferenceType;

            // Iterate over each variable in the field declaration
            foreach (var variable in node.Declaration.Variables)
            {
                var fieldName = variable.Identifier.Text;
                var initializationValue = variable.Initializer?.Value?.ToString();

                var resultString =
                    $"{fieldModifiers} {fieldName}{(isNullable ? "?" : "")} : {fieldType}";

                if (initializationValue != null)
                {
                    resultString += $" = {initializationValue}";
                }
                _innerMembers[MemberTypes.Members].Add(resultString);
            }

            CreateEdge(isPredefinedType, typeFullName, isReferenceType);

            base.VisitFieldDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var propertyTypeSyntax = node.Type;
            var isNullable = propertyTypeSyntax is NullableTypeSyntax;
            var isPredefinedType = propertyTypeSyntax is PredefinedTypeSyntax;

            var propertyModifiers = ExtractModifiers(node);
            var propertyName = node.Identifier.Text;
            var propertyType = _semanticModel.GetTypeInfo(node.Type).Type!;
            var accessorList = node.AccessorList;
            var typeFullName = propertyType.ToDisplayString();
            var isReferenceType = propertyType.IsReferenceType;
            var initializationValue = node.Initializer?.Value?.ToString();

            var hasGetter =
                node.ExpressionBody != null
                || (
                    accessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration))
                    ?? false
                );
            var hasSetter =
                accessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration))
                ?? false;

            var resultString =
                $"{propertyModifiers} {propertyName}{(isNullable ? "?" : "")} : {propertyType}";

            if (hasGetter)
            {
                var getterModifiers =
                    accessorList?.Accessors
                        .FirstOrDefault(a => a.IsKind(SyntaxKind.GetAccessorDeclaration))
                        ?.Modifiers.ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(getterModifiers))
                {
                    getterModifiers = $"{getterModifiers} ";
                }

                resultString += $"<<{getterModifiers}get>>";
            }

            if (hasSetter)
            {
                var setterModifiers =
                    accessorList?.Accessors
                        .FirstOrDefault(a => a.IsKind(SyntaxKind.SetAccessorDeclaration))
                        ?.Modifiers.ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(setterModifiers))
                {
                    setterModifiers = $"{setterModifiers} ";
                }

                resultString += $"<<{setterModifiers}set>>";
            }

            if (initializationValue != null)
            {
                resultString += $" = {initializationValue}";
            }

            _innerMembers[MemberTypes.Members].Add(resultString);

            CreateEdge(isPredefinedType, typeFullName, isReferenceType);

            base.VisitPropertyDeclaration(node);
        }

        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            var modifiers = ExtractModifiers(node);
            var nodeDeclarationType = node.Declaration.Type;
            var eventType = _semanticModel.GetTypeInfo(nodeDeclarationType).Type!;

            foreach (var variable in node.Declaration.Variables)
            {
                var eventName = variable.Identifier.Text;
                var isNullable = nodeDeclarationType is NullableTypeSyntax;
                var typeFullName = eventType.ToDisplayString();
                var isPredefinedType = nodeDeclarationType is PredefinedTypeSyntax;
                var isReferenceType = eventType.IsReferenceType;
                var initializationValue = variable.Initializer?.Value?.ToString();

                var resultString =
                    $"{modifiers} <<event>> {eventName}{(isNullable ? "?" : "")} : {eventType}";
                if (initializationValue != null)
                {
                    resultString += $" = {initializationValue}";
                }
                _innerMembers[MemberTypes.Members].Add(resultString);

                CreateEdge(isPredefinedType, typeFullName, isReferenceType);
            }

            base.VisitEventFieldDeclaration(node);
        }

        private void CreateEdge(bool isPredefinedType, string typeFullName, bool isReferenceType)
        {
            if (!isPredefinedType)
            {
                var id = _idSerivice.GetNextId();
                var edge = new JsonEdge(
                    id,
                    typeFullName,
                    _currentTypeFullName!,
                    isReferenceType ? EdgeTypes.Aggregation : EdgeTypes.Composition
                );
                _resultEdges.Add(edge);
            }
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

        public string ExtractModifiers<T>(T node)
            where T : MemberDeclarationSyntax
        {
            var words = node.Modifiers.Select(modifier => modifier.ToString());
            var mappedWords = _modifiersMappingHelper.MapWords(words);

            return string.Join(" ", mappedWords);
        }

        private List<string> ExtractGenericInfo(INamedTypeSymbol symbol)
        {
            return symbol.TypeParameters
                .Select(param =>
                {
                    string constraintString = param.ConstraintTypes.Any()
                        ? $"{param.Name} : {string.Join(", ", param.ConstraintTypes.Select(constraint => constraint.Name))}"
                        : param.Name;

                    return constraintString;
                })
                .ToList();
        }
    }
}
