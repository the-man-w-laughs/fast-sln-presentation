using Presentation.Models.JsonModels.Nodes.Data;

namespace Presentation.Models.JsonModels.Nodes.Flowchart
{
    public interface IFlowchartNode
    {
        public BlockData Data { get; set; }
    }
}
