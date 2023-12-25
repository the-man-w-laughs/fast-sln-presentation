using Presentation.Models.CodeRepresentation.Contracts;

namespace Presentation.Models.CodeRepresentation
{
    public struct ClassModel : IType
    {
        public string Namespace { get; set; }
        public string AccessModifier { get; set; }
        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public bool IsPartial { get; set; }
        public string Name { get; set; }
        public List<string> GenericParameters { get; set; }
        public List<PropertyModel> Properties { get; set; }
        public List<FieldModel> Fields { get; set; }
        public List<MethodModel> Methods { get; set; }

        public override string ToString()
        {
            var genericParametersString =
                GenericParameters != null ? $"<{string.Join(", ", GenericParameters)}>" : "";

            var propertiesString =
                Properties != null
                    ? $"Properties:\n{string.Join("\n", Properties)}"
                    : "No properties";

            var fieldsString =
                Fields != null ? $"Fields:\n{string.Join("\n", Fields)}" : "No fields";

            var methodsString =
                Methods != null ? $"Methods:\n{string.Join("\n", Methods)}" : "No methods";

            var modifiersString = GetModifiersString();

            return $"Namespace: {Namespace}\nClass: {modifiersString}{Name}{genericParametersString}\n{propertiesString}\n{fieldsString}\n{methodsString}";
        }

        private string GetModifiersString()
        {
            var accessModifierString = !string.IsNullOrEmpty(AccessModifier)
                ? $"{AccessModifier} "
                : "";
            var staticString = IsStatic ? "static " : "";
            var abstractString = IsAbstract ? "abstract " : "";
            var sealedString = IsSealed ? "sealed " : "";
            var partialString = IsPartial ? "partial " : "";

            return $"{accessModifierString}{staticString}{abstractString}{sealedString}{partialString}";
        }
    }
}
