namespace Presentation.Models.CodeRepresentation.Contracts
{
    public interface IType
    {
        public string Namespace { get; set; }
        public string AccessModifier { get; set; }
        public string Name { get; set; }
    }
}
