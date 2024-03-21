import { useCallback } from "react";
import { useStore, BaseEdge, getSmoothStepPath } from "reactflow";

function FlowchartEdge({ id, source, target, markerEnd, style, ...props }) {
  const sourceNode = useStore(
    useCallback((store) => store.nodeInternals.get(source), [source])
  );
  const targetNode = useStore(
    useCallback((store) => store.nodeInternals.get(target), [target])
  );

  if (!sourceNode || !targetNode) {
    return null;
  }

  const sx = sourceNode.position.x + sourceNode.width / 2;
  const sy = sourceNode.position.y + sourceNode.height;
  const tx = targetNode.position.x + targetNode.width / 2;
  const ty = targetNode.position.y;

  const sourcePos = "bottom";
  const targetPos = "top";

  const [edgePath] = getSmoothStepPath({
    sourceX: sx,
    sourceY: sy,
    sourcePosition: sourcePos,
    targetPosition: targetPos,
    targetX: tx,
    targetY: ty,
    borderRadius: 0,
    offset: 40,
  });

  return (
    <BaseEdge path={edgePath} markerEnd={markerEnd} style={style} {...props} />
  );
}

export default FlowchartEdge;
