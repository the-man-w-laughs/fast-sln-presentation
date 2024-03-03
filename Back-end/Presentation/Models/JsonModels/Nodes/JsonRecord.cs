using System.Text.Json.Serialization;

namespace Presentation.Models.JsonModels
{
    public class JsonRecord
    {
        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("data")]
        public RecordData Data { get; }

        [JsonPropertyName("type")]
        public string Type { get; } = "recordNode";

        public JsonRecord(
            string id,
            string name,
            string fullName,
            string modifiers,
            List<string> genericInfo
        )
        {
            Id = id;
            Data = new RecordData(name, fullName, modifiers, genericInfo);
        }
    }
}
