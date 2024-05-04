import { createUrl } from "../utils.js";
import FloatingEdge from "./FloatingEdge.js";

function AssociationEdge(props) {
  const style = {
    strokeWidth: 1,
    stroke: "#000000",
  };

  return <FloatingEdge {...props} style={style} />;
}

export default AssociationEdge;
