using System.Text.Json.Serialization;

namespace Presentation.Models.JsonModels
{
    public class JsonStruct
    {
        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("type")]
        public string Type { get; } = "structNode";

        [JsonPropertyName("data")]
        public StructData Data { get; }

        public JsonStruct(
            string id,
            string name,
            string fullName,
            string modifiers,
            List<string> genericInfo
        )
        {
            Id = id;
            Data = new StructData(name, fullName, modifiers, genericInfo);
        }
    }
}
