import { createUrl } from "../utils.js";
import FloatingEdge from "./FloatingEdge.js";

function CompositonEdge(props) {
  const markerEnd = createUrl("RhombusFilled");
  const style = {
    strokeWidth: 1,
    stroke: "#000000",
  };

  return <FloatingEdge {...props} style={style} markerEnd={markerEnd} />;
}

export default CompositonEdge;
