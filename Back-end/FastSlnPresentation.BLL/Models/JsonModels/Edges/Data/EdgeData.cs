using System.Text.Json.Serialization;

namespace FastSlnPresentation.BLL.Models.JsonModels.Edges.Data
{
    public class EdgeData
    {
        [JsonPropertyName("label")]
        public List<string> Label { get; }

        public EdgeData(List<string>? label)
        {
            Label = label ?? new List<string>();
        }
    }
}
