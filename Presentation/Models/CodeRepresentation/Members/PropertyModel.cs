namespace Presentation.Models.CodeRepresentation
{
    public struct PropertyModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string AccessModifier { get; set; }
        public bool IsReadOnly { get; set; }
        public bool HasGetter { get; set; }
        public bool HasSetter { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsStatic { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsOverride { get; set; }
        public bool IsNew { get; set; }
        public bool IsEvent { get; set; }

        public override string ToString()
        {
            var modifiersString = GetModifiersString();

            var readOnlyString = IsReadOnly ? "readonly " : "";
            var accessModifierString = !string.IsNullOrEmpty(AccessModifier)
                ? $"{AccessModifier} "
                : "";

            var getterSetterString = "";

            if (HasGetter && HasSetter)
            {
                getterSetterString = " { get; set; }";
            }
            else if (HasGetter)
            {
                getterSetterString = " { get; }";
            }
            else if (HasSetter)
            {
                getterSetterString = " { set; }";
            }

            return $"{modifiersString}{accessModifierString}{readOnlyString}{Type} {Name}{getterSetterString}";
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
            if (IsEvent)
                modifiers.Add("event");

            return string.Join(" ", modifiers);
        }
    }
}
