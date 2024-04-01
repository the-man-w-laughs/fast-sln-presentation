using System.Text.Json.Serialization;

namespace FastSlnPresentation.BLL.Models.JsonModels
{
    public class RecordData
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("fullName")]
        public string FullName { get; }

        [JsonPropertyName("modifiers")]
        public string Modifiers { get; }

        [JsonPropertyName("genericInfo")]
        public List<string> GenericInfo { get; set; } = new List<string>();

        [JsonPropertyName("methods")]
        public List<string> Methods { get; set; } = new List<string>();

        [JsonPropertyName("members")]
        public List<string> Members { get; set; } = new List<string>();

        [JsonPropertyName("isPredefined")]
        public bool IsPredefined { get; }

        public RecordData(
            string name,
            string fullName,
            string modifiers,
            List<string> genericInfo,
            bool isPredefined = false
        )
        {
            Name = name;
            FullName = fullName;
            Modifiers = modifiers;
            GenericInfo = genericInfo;
            IsPredefined = isPredefined;
        }
    }
}
