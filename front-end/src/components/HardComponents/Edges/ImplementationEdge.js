import { createUrl } from "../utils.js";
import FloatingEdge from "./FloatingEdge.js";

function ImplementationEdge(props) {
  const markerEnd = createUrl("TriangleUnfilled");
  const style = {
    strokeWidth: 1,
    strokeDasharray: [10, 10],
    stroke: "#000000",
  };

  return <FloatingEdge {...props} style={style} markerEnd={markerEnd} />;
}

export default ImplementationEdge;
