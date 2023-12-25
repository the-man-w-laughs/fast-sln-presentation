using Presentation.Models.CodeRepresentation.Contracts;

namespace Presentation.Models.CodeRepresentation
{
    public struct RecordModel : IType
    {
        public string Namespace { get; set; }
        public string AccessModifier { get; set; }
        public string Name { get; set; }
        public List<PropertyModel> Properties { get; set; }
        public List<MethodModel> Methods { get; set; }

        public RecordModel()
        {
            Properties = new List<PropertyModel>();
        }

        public override string ToString()
        {
            var namespaceString = !string.IsNullOrEmpty(Namespace) ? $"{Namespace}." : "";
            var accessModifierString = !string.IsNullOrEmpty(AccessModifier)
                ? $"{AccessModifier} "
                : "";
            var propertiesString =
                Properties != null
                    ? $"Properties:\n{string.Join("\n", Properties)}"
                    : "No properties";
            var methodsString =
                Methods != null ? $"Methods:\n{string.Join("\n", Methods)}" : "No methods";

            return $"{namespaceString}{accessModifierString}record {Name}\n{propertiesString}\n{methodsString}";
        }
    }
}
