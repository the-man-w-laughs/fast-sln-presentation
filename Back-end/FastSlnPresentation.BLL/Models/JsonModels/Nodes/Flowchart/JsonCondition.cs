using System.Text.Json.Serialization;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes.Data;
using FastSlnPresentation.BLL.Models.JsonModels.Nodes.Flowchart;

namespace FastSlnPresentation.BLL.Models.JsonModels
{
    public class JsonCondition : INode, IFlowchartNode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; } = "conditionNode";

        [JsonPropertyName("data")]
        public BlockData Data { get; set; }

        public JsonCondition() { }

        public JsonCondition(string id, string content)
        {
            var contentList = new List<string>() { content };
            Id = id;
            Data = new BlockData(contentList);
        }

        public JsonCondition(string id, List<string> Content)
        {
            Id = id;
            Data = new BlockData(Content);
        }
    }
}
