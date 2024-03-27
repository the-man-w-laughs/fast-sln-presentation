using System.Text.Json.Serialization;

namespace FastSlnPresentation.BLL.Models.JsonModels.Nodes.Data
{
    public class ClassData
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("fullName")]
        public string FullName { get; }

        [JsonPropertyName("modifiers")]
        public string Modifiers { get; }

        [JsonPropertyName("genericInfo")]
        public List<string> GenericInfo { get; }

        [JsonPropertyName("methods")]
        public List<string> Methods { get; set; } = new List<string>();

        [JsonPropertyName("members")]
        public List<string> Members { get; set; } = new List<string>();

        public ClassData(string name, string fullName, string modifiers, List<string> genericInfo)
        {
            Name = name;
            FullName = fullName;
            Modifiers = modifiers;
            GenericInfo = genericInfo;
        }
    }
}
