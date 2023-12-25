namespace Presentation.Models.CodeRepresentation
{
    public struct FieldModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string AccessModifier { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsStatic { get; set; }
        public bool IsEvent { get; set; }

        public override string ToString()
        {
            var modifiersString = GetModifiersString();

            var accessModifierString = !string.IsNullOrEmpty(AccessModifier)
                ? $"{AccessModifier} "
                : "";

            return $"{modifiersString}{accessModifierString}{Type} {Name}";
        }

        private string GetModifiersString()
        {
            var modifiers = new List<string>();

            if (IsStatic)
                modifiers.Add("static");
            if (IsReadonly)
                modifiers.Add("readonly");
            if (IsEvent)
                modifiers.Add("event");

            return string.Join(" ", modifiers);
        }
    }
}
