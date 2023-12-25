namespace Presentation.Models.CodeRepresentation.Members
{
    public struct ParameterModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Modifier { get; set; }

        public override string ToString()
        {
            return $"{Modifier} {Type} {Name}";
        }
    }
}
