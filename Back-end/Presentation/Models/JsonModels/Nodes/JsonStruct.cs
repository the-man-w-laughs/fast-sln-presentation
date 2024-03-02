namespace Presentation.Models.JsonModels
{
    public class JsonStruct
    {
        public string Id { get; }
        public string Name { get; }
        public string FullName { get; }
        public string Modifiers { get; }
        public string Type { get; } = "structNode";
        public List<string> GenericInfo { get; set; } = new List<string>();
        public List<string> Methods { get; set; } = new List<string>();
        public List<string> Members { get; set; } = new List<string>();

        public JsonStruct(
            string id,
            string name,
            string fullName,
            string modifiers,
            List<string> genericInfo
        )
        {
            Id = id;
            Name = name;
            FullName = fullName;
            Modifiers = modifiers;
            Id = fullName;
            GenericInfo = genericInfo;
        }
    }
}
