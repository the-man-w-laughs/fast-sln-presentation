import { createUrl } from "../utils.js";
import FloatingEdge from "./FloatingEdge.js";

function AggregationEdge(props) {
  const markerEnd = createUrl("RhombusUnfilled");
  const style = {
    strokeWidth: 1,
    stroke: "#000000",
  };

  return <FloatingEdge {...props} style={style} markerEnd={markerEnd} />;
}

export default AggregationEdge;
