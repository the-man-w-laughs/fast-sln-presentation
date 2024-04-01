using System.Text.Json.Serialization;

namespace FastSlnPresentation.BLL.Models.JsonModels
{
    public class EnumData
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("fullName")]
        public string FullName { get; }

        [JsonPropertyName("modifiers")]
        public string Modifiers { get; }

        [JsonPropertyName("members")]
        public List<string> Members { get; set; } = new List<string>();

        [JsonPropertyName("isPredefined")]
        public bool IsPredefined { get; }

        public EnumData(
            string name,
            string fullName,
            string modifiers,
            List<string> members,
            bool isPredefined = false
        )
        {
            Name = name;
            FullName = fullName;
            Modifiers = modifiers;
            Members = members;
            IsPredefined = isPredefined;
        }
    }
}
