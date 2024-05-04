using System.Text.Json.Serialization;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes.Data;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes.Flowchart;

namespace FastSlnPresentation.BLL.Models.JsonModels
{
    public class JsonBlock : INode, IFlowchartNode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; } = "blockNode";

        [JsonPropertyName("data")]
        public BlockData Data { get; set; }

        public JsonBlock() { }

        public JsonBlock(string id, string content)
        {
            var contentList = new List<string>() { content };
            Id = id;
            Data = new BlockData(contentList);
        }

        public JsonBlock(string id, List<string> content)
        {
            Id = id;
            Data = new BlockData(content);
        }
    }
}
