using Presentation.Models.CodeRepresentation.Contracts;
using Presentation.Models.CodeRepresentation.Members;

namespace Presentation.Models.CodeRepresentation
{
    public struct EnumModel : IType
    {
        public string Namespace { get; set; }
        public string AccessModifier { get; set; }
        public string Name { get; set; }
        public string UnderlyingType { get; set; }
        public List<EnumValueModel> EnumValues { get; set; }

        public EnumModel()
        {
            EnumValues = new List<EnumValueModel>();
        }

        public override string ToString()
        {
            var namespaceString = !string.IsNullOrEmpty(Namespace) ? $"{Namespace}." : "";
            var accessModifierString = !string.IsNullOrEmpty(AccessModifier)
                ? $"{AccessModifier} "
                : "";
            var underlyingTypeString = !string.IsNullOrEmpty(UnderlyingType)
                ? $": {UnderlyingType}"
                : "";
            var enumValuesString =
                EnumValues != null
                    ? $"Enum Values:\n{string.Join(", ", EnumValues)}"
                    : "No enum values";

            return $"{namespaceString}{accessModifierString}enum {Name}{underlyingTypeString}\n{enumValuesString}";
        }
    }
}
