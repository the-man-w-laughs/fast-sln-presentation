using System.Text.Json.Serialization;
using Presentation.Models.JsonModels.Nodes;

namespace Presentation.Models.JsonModels
{
    public class JsonDelegate : INode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; } = "delegateNode";

        [JsonPropertyName("data")]
        public DelegateData Data { get; }

        public JsonDelegate(
            string id,
            string name,
            string fullName,
            string modifiers,
            List<string> genericInfo,
            string returnType,
            List<string> parameters
        )
        {
            Id = id;
            Data = new DelegateData(name, fullName, modifiers, genericInfo, returnType, parameters);
        }
    }
}
