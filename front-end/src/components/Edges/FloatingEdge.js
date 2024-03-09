import { useCallback } from "react";
import {
  useStore,
  getStraightPath,
  BaseEdge,
  MarkerType,
  getBezierPath,
  getSmoothStepPath,
  getSimpleBezierPath,
  getMarkerEnd,
} from "reactflow";

import { createUrl } from "../utils.js";
import TriangleUnfilledArrow from "../Markers/TriangleUnfilledArrow.js";

import { getEdgeParams } from "../utils.js";

function FloatingEdge({
  id,
  source,
  target,
  markerStart,
  markerEnd,
  style,
  rfId,
  ...props
}) {
  const sourceNode = useStore(
    useCallback((store) => store.nodeInternals.get(source), [source])
  );
  const targetNode = useStore(
    useCallback((store) => store.nodeInternals.get(target), [target])
  );

  if (!sourceNode || !targetNode) {
    return null;
  }

  const { sx, sy, tx, ty, sourcePos, targetPos } = getEdgeParams(
    sourceNode,
    targetNode
  );

  const [edgePath] = getStraightPath({
    sourceX: sx,
    sourceY: sy,
    sourcePosition: sourcePos,
    targetPosition: targetPos,
    targetX: tx,
    targetY: ty,
  });

  style = {
    strokeWidth: 1,
    strokeDasharray: [5, 5],
    stroke: "#FF0072",
  };

  markerEnd = createUrl("TriangleUnfilledArrow");

  return (
    <>
      <BaseEdge
        path={edgePath}
        markerEnd={markerEnd}
        style={style}
        {...props}
      />
    </>
  );
}

export default FloatingEdge;
