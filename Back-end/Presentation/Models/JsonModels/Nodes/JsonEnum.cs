namespace Presentation.Models.JsonModels
{
    public class JsonEnum
    {
        public string Id { get; }
        public string Name { get; }
        public string FullName { get; }
        public string Modifiers { get; }
        public string Type { get; set; } = "enumNode";
        public List<string> Members { get; set; } = new List<string>();

        public JsonEnum(
            string id,
            string name,
            string fullName,
            string modifiers,
            List<string> members
        )
        {
            Id = id;
            Name = name;
            FullName = fullName;
            Modifiers = modifiers;
            Members = members;
        }
    }
}
