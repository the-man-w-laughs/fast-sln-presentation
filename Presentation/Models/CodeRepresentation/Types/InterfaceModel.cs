using Presentation.Models.CodeRepresentation.Contracts;

namespace Presentation.Models.CodeRepresentation
{
    public struct InterfaceModel : IType
    {
        public string Namespace { get; set; }
        public string AccessModifier { get; set; }
        public string Name { get; set; }
        public List<MethodModel> Methods { get; set; }

        public InterfaceModel()
        {
            Methods = new List<MethodModel>();
        }

        public override string ToString()
        {
            var namespaceString = !string.IsNullOrEmpty(Namespace) ? $"{Namespace}." : "";
            var accessModifierString = !string.IsNullOrEmpty(AccessModifier)
                ? $"{AccessModifier} "
                : "";
            var methodsString =
                Methods != null ? $"Methods:\n{string.Join("\n", Methods)}" : "No methods";

            return $"{namespaceString}{accessModifierString}interface {Name}\n{methodsString}";
        }
    }
}
