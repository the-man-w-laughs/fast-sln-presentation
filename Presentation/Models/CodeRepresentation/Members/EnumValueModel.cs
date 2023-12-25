namespace Presentation.Models.CodeRepresentation.Members
{
    public struct EnumValueModel
    {
        public string Name { get; set; }
        public int? Value { get; set; }

        public override string ToString()
        {
            return Value.HasValue ? $"{Name} = {Value}" : Name;
        }
    }
}
