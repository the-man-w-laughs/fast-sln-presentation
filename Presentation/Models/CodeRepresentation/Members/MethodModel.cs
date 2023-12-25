using Presentation.Models.CodeRepresentation.Members;

namespace Presentation.Models.CodeRepresentation
{
    public struct MethodModel
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public string AccessModifier { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsStatic { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsOverride { get; set; }
        public bool IsNew { get; set; }

        public List<ParameterModel> Parameters { get; set; }

        public override string ToString()
        {
            var accessModifierString = !string.IsNullOrEmpty(AccessModifier)
                ? $"{AccessModifier} "
                : "";

            var parametersString = Parameters != null ? $"({string.Join(", ", Parameters)})" : "()";

            var modifiersString = GetModifiersString();

            return $"{accessModifierString} {modifiersString} {ReturnType} {Name}{parametersString}";
        }

        private string GetModifiersString()
        {
            var modifiers = new List<string>();

            if (IsAbstract)
                modifiers.Add("abstract");
            if (IsStatic)
                modifiers.Add("static");
            if (IsVirtual)
                modifiers.Add("virtual");
            if (IsOverride)
                modifiers.Add("override");
            if (IsNew)
                modifiers.Add("new");

            return string.Join(" ", modifiers);
        }
    }
}
