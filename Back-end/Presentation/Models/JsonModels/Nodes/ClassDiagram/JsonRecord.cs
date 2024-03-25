using System.Text.Json.Serialization;
using Presentation.Models.JsonModels.Nodes;

namespace Presentation.Models.JsonModels
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
            string modifiers,
            List<string> genericInfo
        )
        {
            Id = id;
            Data = new RecordData(name, fullName, modifiers, genericInfo);
        }
    }
}
