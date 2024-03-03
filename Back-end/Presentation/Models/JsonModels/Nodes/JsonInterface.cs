using System.Text.Json.Serialization;

namespace Presentation.Models.JsonModels
{
    public class JsonInterface
    {
        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "interfaceNode";

        [JsonPropertyName("data")]
        public InterfaceData Data { get; }

        public JsonInterface(
            string id,
            string name,
            string fullName,
            string modifiers,
            List<string> genericInfo
        )
        {
            Id = id;
            Data = new InterfaceData(name, fullName, modifiers, genericInfo);
        }
    }
}
