import { useCallback } from "react";
import { useStore, BaseEdge, getSmoothStepPath, useNodes } from "reactflow";
import { pointsToSVG } from "../utils";
import {
  getSmartEdge,
  svgDrawStraightLinePath,
  pathfindingJumpPointNoDiagonal,
} from "@tisoap/react-flow-smart-edge";

function SmartFlowchartEdge({
  id,
  source,
  target,
  markerEnd,
  style,
  ...props
}) {
  const nodes = useNodes();
  const {
    sourcePosition,
    targetPosition,
    sourceX,
    sourceY,
    targetX,
    targetY,
    markerStart,
  } = props;

  const smartOptions = {
    drawEdge: svgDrawStraightLinePath,
    generatePath: pathfindingJumpPointNoDiagonal,
    nodePadding: 15,
    gridRatio: 15,
  };

  const getSmartEdgeResponse = getSmartEdge({
    sourcePosition,
    targetPosition,
    sourceX,
    sourceY,
    targetX,
    targetY,
    nodes,
    options: smartOptions,
  });

  if (getSmartEdgeResponse === null) {
    return <BaseEdge {...props} />;
  }

  const { edgeCenterX, edgeCenterY, svgPathString } = getSmartEdgeResponse;

  console.log(svgPathString);
  let points = svgPathString.split(/\s+/);
  console.log(points);
  // points.splice(3, 3);
  // points.splice(-6, 3);
  let newPathString = points.join(" ");
  console.log(newPathString, "new");

  return (
    <path
      className="react-flow__edge-path"
      d={newPathString}
      markerEnd={markerEnd}
      markerStart={markerStart}
      style={style}
      {...props}
    />
  );
}

export default SmartFlowchartEdge;
