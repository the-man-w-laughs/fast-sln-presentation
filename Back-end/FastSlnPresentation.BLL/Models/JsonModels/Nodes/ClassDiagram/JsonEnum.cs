using System.Text.Json.Serialization;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;

namespace FastSlnPresentation.BLL.Models.JsonModels
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
            string? modifiers = null,
            List<string>? members = null,
            bool isPredefined = false
        )
        {
            Id = id;
            Data = new EnumData(name, fullName, modifiers, members, isPredefined);
        }
    }
}
