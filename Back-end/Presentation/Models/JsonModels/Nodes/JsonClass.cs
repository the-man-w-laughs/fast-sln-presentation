using System.Text.Json.Serialization;
using Presentation.Models.JsonModels.Nodes.Data;

namespace Presentation.Models.JsonModels
{
    public class JsonClass
    {
        [JsonPropertyName("id")]
        public string Id { get; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = "classNode";

        [JsonPropertyName("data")]
        public ClassData Data { get; }

        public JsonClass(
            string id,
            string name,
            string fullName,
            string modifiers,
            List<string> genericInfo
        )
        {
            Id = id;
            Data = new ClassData(name, fullName, modifiers, genericInfo);
        }
    }
}
