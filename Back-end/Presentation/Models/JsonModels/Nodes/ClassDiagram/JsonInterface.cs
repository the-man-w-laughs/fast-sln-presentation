using System.Text.Json.Serialization;
using Presentation.Models.JsonModels.Nodes;

namespace Presentation.Models.JsonModels
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
            string modifiers,
            List<string> genericInfo
        )
        {
            Id = id;
            Data = new InterfaceData(name, fullName, modifiers, genericInfo);
        }
    }
}
