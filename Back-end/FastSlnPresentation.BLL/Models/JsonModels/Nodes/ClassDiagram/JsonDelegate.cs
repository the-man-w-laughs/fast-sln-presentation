using System.Text.Json.Serialization;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;

namespace FastSlnPresentation.BLL.Models.JsonModels
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
            string? modifiers = null,
            List<string>? genericInfo = null,
            string? returnType = null,
            List<string>? parameters = null,
            bool isPredefined = false
        )
        {
            Id = id;
            Data = new DelegateData(
                name,
                fullName,
                modifiers,
                genericInfo,
                returnType,
                parameters,
                isPredefined
            );
        }
    }
}
