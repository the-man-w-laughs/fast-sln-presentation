import { useCallback } from "react";
import {
  useStore,
  getBezierPath,
  getStraightPath,
  getSimpleBezierPath,
  BaseEdge,
} from "reactflow";

import { getEdgeParams } from "./utils.js";

function FloatingEdge({ id, source, target, markerEnd, style }) {
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

  const [edgePath] = getBezierPath({
    sourceX: sx,
    sourceY: sy,
    sourcePosition: sourcePos,
    targetPosition: targetPos,
    targetX: tx,
    targetY: ty,
  });

  return <BaseEdge path={edgePath} markerEnd={markerEnd} style={style} />;
}

export default FloatingEdge;
