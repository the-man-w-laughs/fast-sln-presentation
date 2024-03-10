import { useCallback } from "react";
import { useStore, getStraightPath, BaseEdge } from "reactflow";
import { getEdgeParams } from "../utils.js";

function FloatingEdge({ id, source, target, markerEnd, style, ...props }) {
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

  return (
    <BaseEdge path={edgePath} markerEnd={markerEnd} style={style} {...props} />
  );
}

export default FloatingEdge;
