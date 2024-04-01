using System.Text.Json.Serialization;

namespace FastSlnPresentation.BLL.Models.JsonModels
{
    public class DelegateData
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("fullName")]
        public string FullName { get; }

        [JsonPropertyName("modifiers")]
        public string Modifiers { get; }

        [JsonPropertyName("returnType")]
        public string ReturnType { get; }

        [JsonPropertyName("genericInfo")]
        public List<string> GenericInfo { get; set; } = new List<string>();

        [JsonPropertyName("parameters")]
        public List<string> Parameters { get; set; } = new List<string>();

        [JsonPropertyName("isPredefined")]
        public bool IsPredefined { get; }

        public DelegateData(
            string name,
            string fullName,
            string modifiers,
            List<string> genericInfo,
            string returnType,
            List<string> parameters,
            bool isPredefined = false
        )
        {
            Name = name;
            FullName = fullName;
            Modifiers = modifiers;
            GenericInfo = genericInfo;
            ReturnType = returnType;
            Parameters = parameters;
            IsPredefined = isPredefined;
        }
    }
}
