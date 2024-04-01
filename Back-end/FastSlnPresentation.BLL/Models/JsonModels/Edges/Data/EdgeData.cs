using System.Text.Json.Serialization;

namespace FastSlnPresentation.BLL.Models.JsonModels.Edges.Data
{
    public class EdgeData
    {
        [JsonPropertyName("label")]
        public List<string> Label { get; }

        [JsonPropertyName("targetLabel")]
        public List<string>? TargetLabel { get; }

        [JsonPropertyName("sourceLabel")]
        public List<string>? SourceLabel { get; }

        public EdgeData(List<string>? label, List<string>? targetLabel, List<string>? sourceLabel)
        {
            Label = label ?? new List<string>();
            TargetLabel = targetLabel ?? new List<string>();
            SourceLabel = sourceLabel ?? new List<string>();
        }
    }
}
