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
    public class SourceToJsonWalker : CSharpSyntaxWalker, ISourceCodeToXmlWalker
    {
        // resulting field
        private XmlElement _xmlElement;
        private readonly List<object> _resultNodes = new();
        private readonly List<object> _resultEdges = new();

        // services
        private readonly IIdSerivice _idSerivice;

        // inner fields
        private SemanticModel _semanticModel;
        private readonly SyntaxNode _root;
        private int _nestingDepth = 0;
        private readonly List<SyntaxNode> _innerTypes = new();
        private readonly Dictionary<List<string>> _innerMembers = new();

        private XmlElement? _currentTypeElement;

        public SourceToJsonWalker(
            SemanticModel semanticModel,
            SyntaxNode root,
            XmlElement xmlElement,
            IIdSerivice idSerivice
        )
        {
            _semanticModel = semanticModel;
            _root = root;
            _xmlElement = xmlElement;
            _idSerivice = idSerivice;
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
            var node = new JsonClass(fullName, name, fullName, modifiersStr, genericInfo);

            // to delete
            var classElement = _xmlElement.AppendClass();
            _currentTypeElement = classElement;

            _nestingDepth++;
            AddInheritanceFrom(node);
            _innerMembers.Clear();
            base.VisitClassDeclaration(node);
            node.Members.Add(_innerMembers[MemberTypes.Members]);
            node.Methods.Add(_innerMembers[MemberTypes.Methods]);
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
            var name = symbol.Name;
            var fullName = symbol.ContainingNamespace.ToDisplayString();
            var genericInfo = ExtractGenericInfo(symbol);
            var modifiersStr = ExtractModifiers(node);

            var node = new JsonInterface(
                fullName,
                name,
                fullName,
                modifiersStr,
                genericInfo
            );

            // Write information

            // to delete
            var interfaceElement = _xmlElement!.AppendInterface();
            _currentTypeElement = interfaceElement;

            _nestingDepth++;
            AddInheritanceFrom(node);
            _innerMembers.Clear();
            base.VisitInterfaceDeclaration(node);
            node.Members.Add(_innerMembers[MemberTypes.Members]);
            node.Methods.Add(_innerMembers[MemberTypes.Methods]);
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
            var fullName = symbol.ContainingNamespace.ToDisplayString();
            List<string> genericInfo = ExtractGenericInfo(symbol);
            string modifiersStr = ExtractModifiers(node);

            var node = new JsonStruct(
                fullName,
                structName,
                fullName,
                modifiersStr,
                genericInfo
            );

            // Write information into XML or other data structure

            // to delete
            var structElement = _xmlElement!.AppendStruct();
            _currentTypeElement = structElement;

            _nestingDepth++;
            _innerMembers.Clear();
            AddInheritanceFrom(node);
            base.VisitStructDeclaration(node);
            node.Members.Add(_innerMembers[MemberTypes.Members]);
            node.Methods.Add(_innerMembers[MemberTypes.Methods]);
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
            var fullName = symbol.ToDisplayString();
            var modifiersStr = ExtractModifiers(node);
            var members = new List<string>();

            foreach (var member in node.Members)
            {
                var memberName = member.Identifier.Text;
                var memberValue = member.EqualsValue?.Value;

                members.Add($"{memberName}{(memberValue == null ? "" : $" = {memberValue}")}");
            }

            var enumNode = new JsonEnum(fullName, enumName, fullName, modifiersStr, members);

            // to Delete
            var enumElement = _xmlElement!.AppendEnum();
            _currentTypeElement = enumElement;

            _nestingDepth++;
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

            var recordName = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var modifiersStr = ExtractModifiers(node);
            var genericInfo = ExtractGenericInfo(symbol);

            var node = new JsonRecord(fullName, recordName, fullName, modifiersStr, genericInfo)

            _nestingDepth++;
            // to Delete
            var recordElement = _xmlElement!.AppendRecord();
            _currentTypeElement = recordElement;

            AddInheritanceFrom(node);
            _innerMembers.Clear();
            base.VisitRecordDeclaration(node);
            node.Members.Add(_innerMembers[MemberTypes.Members]);
            node.Methods.Add(_innerMembers[MemberTypes.Methods]);
            _nestingDepth--;
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
                parameters.Add($"{paramName} : {paramType}")                                   
            }            

            var delegateNode = new JsonDelegate(fullName, delegateName, fullName, modifiersStr, genericInfo, returnType, parameters);

            _nestingDepth++;
            // to delete
            var delegateElement = _xmlElement!.AppendDelegate();
            _currentTypeElement = delegateElement;
            base.VisitDelegateDeclaration(node);
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
                }
            }

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
                var edge = new JsonEdge(
                    id,
                    baseType.ToDisplayString(),
                    symbol.ToDisplayString(),
                    edgeType
                );
                _resultEdges.Add(edge);
            }
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

        private static string ExtractModifiers(TypeDeclarationSyntax node)
        {
            return string.Join(" ", node.Modifiers.Select(modifier => modifier.ToString()));
        }

        private static string ExtractModifiers(EnumDeclarationSyntax node)
        {
            return string.Join(" ", node.Modifiers.Select(modifier => modifier.ToString()));
        }

        private static List<string> ExtractGenericInfo(INamedTypeSymbol symbol)
        {
            return symbol.TypeParameters
                .Select(param =>
                {
                    var constraintString = string.Join(
                        ", ",
                        param.ConstraintTypes.Select(constraint => constraint.Name)
                    );
                    return $"{param.Name} : {constraintString}";
                })
                .ToList();
        }
    }
}
