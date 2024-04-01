using System.Text.Json.Serialization;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;

namespace FastSlnPresentation.BLL.Models.JsonModels
{
    public class JsonRecord : INode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("data")]
        public RecordData Data { get; }

        [JsonPropertyName("type")]
        public string Type { get; } = "recordNode";

        public JsonRecord(
            string id,
            string name,
            string fullName,
            string? modifiers = null,
            List<string>? genericInfo = null,
            bool isPredefined = false
        )
        {
            Id = id;
            Data = new RecordData(name, fullName, modifiers, genericInfo, isPredefined);
        }
    }
}
