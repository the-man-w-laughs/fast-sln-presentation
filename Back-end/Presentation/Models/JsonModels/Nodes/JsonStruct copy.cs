namespace Presentation.Models.JsonModels
{
    public class JsonDelegate
    {
        public string Id { get; }
        public string Name { get; }
        public string FullName { get; }
        public string Modifiers { get; }
        public string ReturnType { get; }
        public string Type { get; } = "delegateNode";
        public List<string> GenericInfo { get; set; } = new List<string>();
        public List<string> Parameters { get; set; } = new List<string>();

        public JsonDelegate(
            string id,
            string name,
            string fullName,
            string modifiers,
            List<string> genericInfo,
            string returnType,
            List<string> parameters
        )
        {
            Id = id;
            Name = name;
            FullName = fullName;
            Modifiers = modifiers;
            Id = fullName;
            GenericInfo = genericInfo;
            ReturnType = returnType;
            Parameters = parameters;
        }
    }
}
