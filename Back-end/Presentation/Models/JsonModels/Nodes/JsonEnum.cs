using System.Text.Json.Serialization;

namespace Presentation.Models.JsonModels
{
    public class JsonEnum
    {
        [JsonPropertyName("id")]
        public string Id { get; }

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
