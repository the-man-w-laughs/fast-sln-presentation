using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using FastSlnPresentation.BLL.Consts;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Models.JsonModels;
using FastSlnPresentation.BLL.Models.JsonModels.Edges;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using FastSlnPresentation.BLL.Services;

namespace FastSlnPresentation.BLL.SyntaxWalkers
{
    // Обходчик синтаксического дерева для преобразования классов в JSON
    public class ClassToJsonWalker : CSharpSyntaxWalker, ISourceToJsonWalker
    {
        // Результирующие поля
        private readonly List<INode> _resultNodes;
        private readonly List<JsonEdge> _resultEdges;

        // Сервисы
        private readonly IIdService _idSerivice;

        // Внутренние поля
        private SemanticModel _semanticModel;
        private readonly SyntaxNode _root;
        private int _nestingDepth = 0;
        private readonly List<SyntaxNode> _innerTypes = new();
        private readonly Dictionary<string, List<string>> _innerMembers = new();
        private string? _currentTypeFullName;

        // Конструктор
        public ClassToJsonWalker(
            List<INode> nodes,
            List<JsonEdge> edges,
            SemanticModel semanticModel,
            SyntaxNode root,
            IIdService idSerivice
        )
        {
            _resultNodes = nodes;
            _resultEdges = edges;
            _semanticModel = semanticModel;
            _root = root;
            _idSerivice = idSerivice;
        }

        // Метод для запуска обхода
        public void Parse()
        {
            Visit(_root);
            VisitInternalTypes();
        }

        // Переопределение метода посещения объявления класса
        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            // Пропускаем внутренние классы
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            // Получаем символ класса
            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            // Извлекаем информацию о классе
            var name = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var genericInfo = ExtractGenericInfo(symbol);
            var modifiersStr = ExtractModifiers(node);

            // Создаем узел JSON
            var jsonNode = new JsonClass(fullName, name, fullName, modifiersStr, genericInfo);

            // Увеличиваем уровень вложенности
            _nestingDepth++;

            // Добавляем информацию о наследовании
            AddInheritanceFrom(node);

            // Инициализируем внутренние члены класса
            _innerMembers[MemberTypes.Members] = new();
            _innerMembers[MemberTypes.Methods] = new();
            _currentTypeFullName = fullName;

            // Продолжаем обход внутренних элементов класса
            base.VisitClassDeclaration(node);

            // Добавляем внутренние члены к узлу JSON
            jsonNode.Data.Members.AddRange(_innerMembers[MemberTypes.Members]);
            jsonNode.Data.Methods.AddRange(_innerMembers[MemberTypes.Methods]);

            // Уменьшаем уровень вложенности
            _nestingDepth--;

            // Добавляем узел JSON в список результатов
            _resultNodes.Add(jsonNode);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            // Пропускаем внутренние интерфейсы
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            // Получаем символ интерфейса
            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            // Извлекаем информацию о интерфейсе
            var name = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var genericInfo = ExtractGenericInfo(symbol);
            var modifiersStr = ExtractModifiers(node);

            // Создаем узел JSON для интерфейса
            var jsonNode = new JsonInterface(fullName, name, fullName, modifiersStr, genericInfo);

            // Увеличиваем уровень вложенности
            _nestingDepth++;

            // Добавляем информацию о наследовании
            AddInheritanceFrom(node);

            // Инициализируем внутренние члены интерфейса
            _innerMembers[MemberTypes.Members] = new();
            _innerMembers[MemberTypes.Methods] = new();
            _currentTypeFullName = fullName;

            // Продолжаем обход внутренних элементов интерфейса
            base.VisitInterfaceDeclaration(node);

            // Добавляем внутренние члены к узлу JSON
            jsonNode.Data.Members.AddRange(_innerMembers[MemberTypes.Members]);
            jsonNode.Data.Methods.AddRange(_innerMembers[MemberTypes.Methods]);

            // Уменьшаем уровень вложенности
            _nestingDepth--;

            // Добавляем узел JSON в список результатов
            _resultNodes.Add(jsonNode);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            // Пропускаем внутренние структуры
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            // Получаем символ структуры
            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            // Извлекаем информацию о структуре
            var structName = symbol.Name;
            var fullName = symbol.ContainingNamespace.ToDisplayString();
            List<string> genericInfo = ExtractGenericInfo(symbol);
            string modifiersStr = ExtractModifiers(node);

            // Создаем узел JSON для структуры
            var jsonNode = new JsonStruct(
                fullName,
                structName,
                fullName,
                modifiersStr,
                genericInfo
            );

            // Увеличиваем уровень вложенности
            _nestingDepth++;

            // Добавляем информацию о наследовании
            AddInheritanceFrom(node);

            // Инициализируем внутренние члены структуры
            _innerMembers[MemberTypes.Members] = new();
            _innerMembers[MemberTypes.Methods] = new();
            _currentTypeFullName = fullName;

            // Продолжаем обход внутренних элементов структуры
            base.VisitStructDeclaration(node);

            // Добавляем внутренние члены к узлу JSON
            jsonNode.Data.Members.AddRange(_innerMembers[MemberTypes.Members]);
            jsonNode.Data.Methods.AddRange(_innerMembers[MemberTypes.Methods]);

            // Уменьшаем уровень вложенности
            _nestingDepth--;

            // Добавляем узел JSON в список результатов
            _resultNodes.Add(jsonNode);
        }
        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            // Пропускаем внутренние перечисления
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            // Получаем символ перечисления
            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            // Извлекаем информацию о перечислении
            var enumName = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var modifiersStr = ExtractModifiers(node);
            var members = new List<string>();

            // Обрабатываем члены перечисления
            foreach (var member in node.Members)
            {
                var memberName = member.Identifier.Text;
                var memberValue = member.EqualsValue?.Value;

                members.Add($"{memberName}{(memberValue == null ? "" : $" = {memberValue}")}");
            }

            // Создаем узел JSON для перечисления
            var jsonNode = new JsonEnum(fullName, enumName, fullName, modifiersStr, members);

            // Увеличиваем уровень вложенности
            _nestingDepth++;

            // Продолжаем обход внутренних элементов перечисления
            base.VisitEnumDeclaration(node);

            // Уменьшаем уровень вложенности
            _nestingDepth--;

            // Добавляем узел JSON в список результатов
            _resultNodes.Add(jsonNode);
        }

        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            // Пропускаем внутренние записи
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            // Получаем символ записи
            var symbol = _semanticModel.GetDeclaredSymbol(node)!;

            // Извлекаем информацию о записи
            var recordName = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var modifiersStr = ExtractModifiers(node);
            var genericInfo = ExtractGenericInfo(symbol);

            // Создаем узел JSON для записи
            var jsonNode = new JsonRecord(
                fullName,
                recordName,
                fullName,
                modifiersStr,
                genericInfo
            );

            // Увеличиваем уровень вложенности
            _nestingDepth++;

            // Добавляем информацию о наследовании
            AddInheritanceFrom(node);

            // Инициализируем внутренние члены записи
            _innerMembers[MemberTypes.Members] = new();
            _innerMembers[MemberTypes.Members] = new();
            _currentTypeFullName = fullName;

            // Продолжаем обход внутренних элементов записи
            base.VisitRecordDeclaration(node);

            // Добавляем внутренние члены к узлу JSON
            jsonNode.Data.Members.AddRange(_innerMembers[MemberTypes.Members]);
            jsonNode.Data.Methods.AddRange(_innerMembers[MemberTypes.Methods]);

            // Уменьшаем уровень вложенности
            _nestingDepth--;

            // Добавляем узел JSON в список результатов
            _resultNodes.Add(jsonNode);
        }

        public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            // Пропускаем внутренние делегаты
            if (SkipInnerTypeDeclaration(node))
            {
                return;
            }

            // Получаем символ делегата
            var symbol = _semanticModel.GetDeclaredSymbol(node);

            // Извлекаем информацию о делегате
            var delegateName = symbol.Name;
            var fullName = symbol.ToDisplayString();
            var modifiersStr = ExtractModifiers(node);
            var genericInfo = ExtractGenericInfo(symbol);

            // Получаем информацию о методе вызова делегата
            var invokeMethod = symbol.DelegateInvokeMethod!;
            var returnType = invokeMethod.ReturnType.ToDisplayString();

            // Обрабатываем параметры метода вызова делегата
            var parameters = new List<string>();
            foreach (var parameter in invokeMethod.Parameters)
            {
                var paramName = parameter.Name;
                var paramType = parameter.Type.ToDisplayString();
                parameters.Add($"{paramName} : {paramType}");
            }

            // Создаем узел JSON для делегата
            var jsonNode = new JsonDelegate(
                fullName,
                delegateName,
                fullName,
                modifiersStr,
                genericInfo,
                returnType,
                parameters
            );

            // Увеличиваем уровень вложенности
            _nestingDepth++;

            // Продолжаем обход внутренних элементов делегата
            base.VisitDelegateDeclaration(node);

            // Уменьшаем уровень вложенности
            _nestingDepth--;

            // Добавляем узел JSON в список результатов
            _resultNodes.Add(jsonNode);
        }


        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            // Получаем символ метода
            var symbol = _semanticModel.GetDeclaredSymbol(node);

            // Если символ метода не найден, выходим из метода
            if (symbol == null)
                return;

            // Извлекаем информацию из объявления метода
            var methodName = symbol.Name;
            var returnType = symbol.ReturnType?.ToDisplayString() ?? "void";
            string modifiersStr = ExtractModifiers(node);

            // Обрабатываем параметры метода
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

            // Формируем строку с информацией о методе
            var resultString = $"{modifiersStr} {methodName} ({parametersListStr}): {returnType}";
            _innerMembers[MemberTypes.Methods].Add(resultString);
        }

        public void AddInheritanceFrom(TypeDeclarationSyntax type)
        {
            // Если список базовых типов отсутствует, выходим из метода
            if (type.BaseList == null)
                return;

            // Получаем символ типа
            var symbol = _semanticModel.GetDeclaredSymbol(type)!;

            // Обходим каждый базовый тип в списке
            foreach (var baseTypeSyntax in type.BaseList.Types)
            {
                var baseTypeInfo = _semanticModel.GetTypeInfo(baseTypeSyntax.Type);
                var baseType = baseTypeInfo.Type as INamedTypeSymbol;

                // Если базовый тип не найден, выбрасываем исключение
                if (baseType == null)
                    throw new InvalidOperationException(
                        "Не удалось получить информацию о типе базового типа."
                    );

                // Пропускаем типы из других сборок
                if (baseType.ContainingAssembly.Identity != symbol.ContainingAssembly.Identity)
                    continue;

                // Определяем тип ребра (наследование или реализация интерфейса)
                var edgeType =
                    baseType.TypeKind == TypeKind.Interface
                        ? EdgeTypes.Implementation
                        : EdgeTypes.Inheritance;

                // Генерируем уникальный идентификатор ребра
                var id = _idSerivice.GetNextId();
                var targetFullName = baseType.ConstructedFrom.ToDisplayString();
                var sourceFullName = symbol.ToDisplayString();
                var edge = new JsonEdge(id, targetFullName, sourceFullName, edgeType);
                _resultEdges.Add(edge);
            }
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            // Получаем тип поля
            var fieldTypeSyntax = node.Declaration.Type;
            var isNullable = fieldTypeSyntax is NullableTypeSyntax;
            var isPredefinedType = fieldTypeSyntax is PredefinedTypeSyntax;

            // Если тип является предопределенным, создаем соответствующий узел
            if (isPredefinedType)
            {
                CreatePredefinedNode(fieldTypeSyntax);
            }

            // Извлекаем модификаторы поля
            var fieldModifiers = ExtractModifiers(node);
            var fieldType = _semanticModel.GetTypeInfo(fieldTypeSyntax).Type!;

            // Обходим каждую переменную в объявлении поля
            foreach (var variable in node.Declaration.Variables)
            {
                var fieldName = variable.Identifier.Text;
                var initializationValue = variable.Initializer?.Value?.ToString();

                // Формируем строку с информацией о переменной поля
                var resultString =
                    $"{fieldModifiers} {fieldName}{(isNullable ? "?" : "")} : {fieldType}";

                // Добавляем значение инициализации, если оно присутствует
                if (initializationValue != null)
                {
                    resultString += $" = {initializationValue}";
                }
                _innerMembers[MemberTypes.Members].Add(resultString);
            }

            // Создаем ребро для связи с типом поля
            CreateEdge(fieldTypeSyntax);
        }


        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            // Получаем синтаксис типа свойства
            var propertyTypeSyntax = node.Type;
            var isNullable = propertyTypeSyntax is NullableTypeSyntax;
            var isPredefinedType = propertyTypeSyntax is PredefinedTypeSyntax;

            // Если тип является предопределенным, создаем соответствующий узел
            if (isPredefinedType)
            {
                CreatePredefinedNode(propertyTypeSyntax);
            }

            // Извлекаем модификаторы свойства, его имя, тип и список аксессоров
            var propertyModifiers = ExtractModifiers(node);
            var propertyName = node.Identifier.Text;
            var propertyType = _semanticModel.GetTypeInfo(propertyTypeSyntax).Type!;
            var accessorList = node.AccessorList;
            var initializationValue = node.Initializer?.Value?.ToString();

            // Определяем, есть ли геттер и сеттер для свойства
            var hasGetter =
                node.ExpressionBody != null
                || (
                    accessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration))
                    ?? false
                );
            var hasSetter =
                accessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration))
                ?? false;

            // Формируем строку с информацией о свойстве
            var resultString =
                $"{propertyModifiers} {propertyName}{(isNullable ? "?" : "")} : {propertyType}";

            // Добавляем информацию о геттере, если он присутствует
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

            // Добавляем информацию о сеттере, если он присутствует
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

            // Добавляем значение инициализации, если оно присутствует
            if (initializationValue != null)
            {
                resultString += $" = {initializationValue}";
            }

            // Добавляем строку с информацией о свойстве в список членов
            _innerMembers[MemberTypes.Members].Add(resultString);

            // Создаем ребро для связи с типом свойства
            CreateEdge(propertyTypeSyntax);
        }
        private void CreateEdge(TypeSyntax propertyTypeSyntax)
        {
            // Получаем тип свойства из синтаксиса типа
            var propertyType = _semanticModel.GetTypeInfo(propertyTypeSyntax).Type!;
            var typeFullName = propertyType.ToDisplayString();
            var isReferenceType = propertyType.IsReferenceType;

            // Проверяем тип синтаксиса и создаем ребро в зависимости от типа
            if (propertyTypeSyntax is GenericNameSyntax genericTypeSyntax)
            {
                // Для обобщенных типов создаем ребро для каждого аргумента типа
                foreach (var argument in genericTypeSyntax.TypeArgumentList.Arguments)
                {
                    var innerTypeInfo = _semanticModel.GetTypeInfo(argument).Type!;
                    var innerTypeName = innerTypeInfo.ToDisplayString();

                    CreateEdge(innerTypeName.TrimEnd('?'), EdgeTypes.Association);
                }
            }
            else if (propertyTypeSyntax is ArrayTypeSyntax arrayTypeSyntax)
            {
                // Для массивов создаем ребро для типа элементов массива
                var innerTypeInfo = _semanticModel.GetTypeInfo(arrayTypeSyntax.ElementType).Type!;
                var innerTypeName = innerTypeInfo.ToDisplayString();

                CreateEdge(innerTypeName.TrimEnd('?'), EdgeTypes.Association);
            }
            else if (propertyTypeSyntax is TupleTypeSyntax tupleTypeSyntax)
            {
                // Для кортежей создаем ребро для каждого элемента кортежа
                foreach (var element in tupleTypeSyntax.Elements)
                {
                    var innerTypeInfo = _semanticModel.GetTypeInfo(element.Type).Type!;
                    var innerTypeName = innerTypeInfo.ToDisplayString();

                    CreateEdge(innerTypeName.TrimEnd('?'), EdgeTypes.Association);
                }
            }
            else
            {
                // Для остальных типов создаем ребро агрегации или композиции
                CreateEdge(
                    typeFullName.TrimEnd('?'),
                    isReferenceType ? EdgeTypes.Aggregation : EdgeTypes.Composition
                );
            }
        }

        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            // Извлекаем модификаторы события и тип события
            var modifiers = ExtractModifiers(node);
            var nodeDeclarationType = node.Declaration.Type;
            var eventType = _semanticModel.GetTypeInfo(nodeDeclarationType).Type!;

            // Обходим каждую переменную в объявлении события
            foreach (var variable in node.Declaration.Variables)
            {
                var eventName = variable.Identifier.Text;
                var isNullable = nodeDeclarationType is NullableTypeSyntax;
                var typeFullName = eventType.ToDisplayString();
                var isPredefinedType = nodeDeclarationType is PredefinedTypeSyntax;
                var isReferenceType = eventType.IsReferenceType;
                var initializationValue = variable.Initializer?.Value?.ToString();

                // Формируем строку с информацией о событии
                var resultString =
                    $"{modifiers} <<event>> {eventName}{(isNullable ? "?" : "")} : {eventType}";
                if (initializationValue != null)
                {
                    resultString += $" = {initializationValue}";
                }
                _innerMembers[MemberTypes.Members].Add(resultString);

                // Создаем ребро для связи с типом события
                CreateEdge(
                    typeFullName,
                    isReferenceType ? EdgeTypes.Aggregation : EdgeTypes.Composition
                );
            }

            base.VisitEventFieldDeclaration(node);
        }

        private void CreatePredefinedNode(TypeSyntax typeSyntax)
        {
            // Получаем символ типа из синтаксиса
            var typeSymbol = _semanticModel.GetSymbolInfo(typeSyntax).Symbol as INamedTypeSymbol;

            if (typeSymbol != null)
            {
                // Извлекаем имя и полное имя типа
                string typeName = typeSymbol.Name;
                string fullTypeName = typeSymbol.ToDisplayString();

                // Проверяем, не создан ли уже узел с таким полным именем
                if (!_resultNodes.Any(node => node.Id == fullTypeName))
                {
                    INode? typeNode = null;
                    var namedTypeSymbol = GetNamedTypeSymbol(typeSymbol);

                    // Создаем узел в зависимости от типа символа
                    if (namedTypeSymbol != null)
                    {
                        var kind = namedTypeSymbol.TypeKind;
                        switch (kind)
                        {
                            case TypeKind.Class:
                                typeNode = new JsonClass(
                                    fullTypeName,
                                    typeName,
                                    fullTypeName,
                                    isPredefined: true
                                );
                                break;
                            case TypeKind.Interface:
                                typeNode = new JsonInterface(
                                    fullTypeName,
                                    typeName,
                                    fullTypeName,
                                    isPredefined: true
                                );
                                break;
                            case TypeKind.Delegate:
                                typeNode = new JsonDelegate(
                                    fullTypeName,
                                    typeName,
                                    fullTypeName,
                                    isPredefined: true
                                );
                                break;
                            case TypeKind.Struct:
                                typeNode = new JsonStruct(
                                    fullTypeName,
                                    typeName,
                                    fullTypeName,
                                    isPredefined: true
                                );
                                break;
                            case TypeKind.Enum:
                                typeNode = new JsonEnum(
                                    fullTypeName,
                                    typeName,
                                    fullTypeName,
                                    isPredefined: true
                                );
                                break;
                            default:
                                break;
                        }
                    }

                    // Добавляем узел в список результатов, если он был создан
                    if (typeNode != null)
                    {
                        _resultNodes.Add(typeNode);
                    }
                }
            }
        }

        private INamedTypeSymbol? GetNamedTypeSymbol(INamedTypeSymbol symbol)
        {
            // Рекурсивно ищем первый именованный тип символа
            if (symbol.TypeKind != TypeKind.Error)
            {
                return symbol;
            }
            else if (symbol.BaseType != null)
            {
                return GetNamedTypeSymbol(symbol.BaseType);
            }
            else
            {
                return null;
            }
        }

        private void CreateEdge(string typeFullName, string edgeType)
        {
            // Создаем ребро с помощью идентификатора и добавляем его в список ребер
            var id = _idSerivice.GetNextId();
            var edge = new JsonEdge(id, typeFullName, _currentTypeFullName!, edgeType);
            _resultEdges.Add(edge);
        }

        private bool SkipInnerTypeDeclaration(SyntaxNode node)
        {
            // Пропускаем вложенные объявления типов, если текущая глубина вложенности не равна 0
            if (_nestingDepth == 0)
            {
                return false;
            }

            _innerTypes.Add(node);
            return true;
        }

        private void VisitInternalTypes()
        {
            // Обходим внутренние типы и вызываем для них метод Visit
            foreach (var innerType in _innerTypes)
            {
                Visit(innerType);
            }
        }

        public string ExtractModifiers<T>(T node)
            where T : MemberDeclarationSyntax
        {
            // Извлекаем модификаторы из узла и применяем к ним маппинг модификаторов
            var words = node.Modifiers.Select(modifier => modifier.ToString());
            var mappedWords = ModifiersMappingHelper.MapWords(words);

            return string.Join(" ", mappedWords);
        }

        private List<string> ExtractGenericInfo(INamedTypeSymbol symbol)
        {
            // Извлекаем информацию о обобщениях типа
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
