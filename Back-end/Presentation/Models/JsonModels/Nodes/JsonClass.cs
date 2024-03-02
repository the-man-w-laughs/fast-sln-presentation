namespace Presentation.Models.JsonModels
{
    public class JsonClass
    {
        public string Id { get; }
        public string Name { get; }
        public string FullName { get; }
        public string Modifiers { get; }
        public string Type { get; set; } = "classNode";
        public List<string> GenericInfo { get; }
        public List<string> Methods { get; set; } = new List<string>();
        public List<string> Members { get; set; } = new List<string>();

        public JsonClass(
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
            GenericInfo = genericInfo;
        }
    }
}
