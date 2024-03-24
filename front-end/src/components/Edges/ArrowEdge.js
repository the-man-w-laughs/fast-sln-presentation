import { createUrl } from "../utils.js";
import SmartFlowchartEdge from "./SmartFlowchartEdge.js";
import FlowchartEdge from "./FlowchartEdge.js";

function ArrowEdge(props) {
  const markerEnd = createUrl("Arrow");
  const style = {
    strokeWidth: 1,
    stroke: "#000000",
  };

  return <FlowchartEdge {...props} style={style} markerEnd={markerEnd} />;
}

export default ArrowEdge;
