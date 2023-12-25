using Presentation.Models.CodeRepresentation.Contracts;

namespace Presentation.Models.CodeRepresentation
{
    public struct StructModel : IType
    {
        public string Namespace { get; set; }
        public string AccessModifier { get; set; }
        public string Name { get; set; }
        public List<FieldModel> Fields { get; set; }
        public List<PropertyModel> Properties { get; set; }
        public List<MethodModel> Methods { get; set; }

        public StructModel()
        {
            Fields = new List<FieldModel>();
            Methods = new List<MethodModel>();
        }

        public override string ToString()
        {
            var namespaceString = !string.IsNullOrEmpty(Namespace) ? $"{Namespace}." : "";
            var accessModifierString = !string.IsNullOrEmpty(AccessModifier)
                ? $"{AccessModifier} "
                : "";
            var fieldsString =
                Fields != null ? $"Fields:\n{string.Join("\n", Fields)}" : "No fields";
            var methodsString =
                Methods != null ? $"Methods:\n{string.Join("\n", Methods)}" : "No methods";

            return $"{namespaceString}{accessModifierString}struct {Name}\n{fieldsString}\n{methodsString}";
        }
    }
}
