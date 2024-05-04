using System.Text.Json.Serialization;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;

namespace FastSlnPresentation.BLL.Models.JsonModels
{
    public class JsonInterface : INode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; } = "interfaceNode";

        [JsonPropertyName("data")]
        public InterfaceData Data { get; }

        public JsonInterface(
            string id,
            string name,
            string fullName,
            string? modifiers = null,
            List<string>? genericInfo = null,
            bool isPredefined = false
        )
        {
            Id = id;
            Data = new InterfaceData(name, fullName, modifiers, genericInfo, isPredefined);
        }
    }
}
