using FastSlnPresentation.BLL.Models.JsonModels.Nodes.Data;

namespace FastSlnPresentation.BLL.Models.JsonModels.Nodes.Flowchart
{
    public interface IFlowchartNode
    {
        public BlockData Data { get; set; }
    }
}
