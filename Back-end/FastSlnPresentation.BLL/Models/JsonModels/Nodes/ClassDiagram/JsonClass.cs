using System.Text.Json.Serialization;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes.Data;

namespace FastSlnPresentation.BLL.Models.JsonModels
{
    public class JsonClass : INode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; } = "classNode";

        [JsonPropertyName("data")]
        public ClassData Data { get; }

        public JsonClass(
            string id,
            string name,
            string fullName,
            string? modifiers = null,
            List<string>? genericInfo = null,
            bool isPredefined = false
        )
        {
            Id = id;
            Data = new ClassData(name, fullName, modifiers, genericInfo, isPredefined);
        }
    }
}
