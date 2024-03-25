using System.Text.Json.Serialization;
using Presentation.Models.JsonModels.Nodes;
using Presentation.Models.JsonModels.Nodes.Data;
using Presentation.Models.JsonModels.Nodes.Flowchart;

namespace Presentation.Models.JsonModels
{
    public class JsonTerminal : INode, IFlowchartNode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; } = "terminalNode";

        [JsonPropertyName("data")]
        public BlockData Data { get; set; }

        public JsonTerminal() { }

        public JsonTerminal(string id, string content)
        {
            var contentList = new List<string>() { content };
            Id = id;
            Data = new BlockData(contentList);
        }

        public JsonTerminal(string id, List<string> Content)
        {
            Id = id;
            Data = new BlockData(Content);
        }
    }
}
