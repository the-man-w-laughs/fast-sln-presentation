using System.Text.Json.Serialization;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;

namespace FastSlnPresentation.BLL.Models.JsonModels
{
    public class JsonStruct : INode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

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
