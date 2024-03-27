using System.Text.Json.Serialization;

namespace FastSlnPresentation.BLL.Models.JsonModels.Nodes.Data
{
    public class BlockData
    {
        [JsonPropertyName("content")]
        public List<string> Content { get; }

        public BlockData(List<string> content)
        {
            Content = content;
        }
    }
}
