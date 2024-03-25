using System.Text.Json.Serialization;
using Presentation.Models.JsonModels.Nodes;

namespace Presentation.Models.JsonModels
{
    public class JsonEnum : INode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; } = "enumNode";

        [JsonPropertyName("data")]
        public EnumData Data { get; }

        public JsonEnum(
            string id,
            string name,
            string fullName,
            string modifiers,
            List<string> members
        )
        {
            Id = id;
            Data = new EnumData(name, fullName, modifiers, members);
        }
    }
}
