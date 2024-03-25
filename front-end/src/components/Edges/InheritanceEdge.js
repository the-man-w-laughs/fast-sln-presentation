import { createUrl } from "../utils.js";
import FloatingEdge from "./FloatingEdge.js";

function InheritanceEdge(props) {
  const markerEnd = createUrl("TriangleUnfilled");
  const style = {
    strokeWidth: 1,
    stroke: "#000000",
  };

  return <FloatingEdge {...props} style={style} markerEnd={markerEnd} />;
}

export default InheritanceEdge;
