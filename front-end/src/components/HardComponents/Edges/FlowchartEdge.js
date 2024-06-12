import { useCallback } from "react";
import {
  useStore,
  BaseEdge,
  getSmoothStepPath,
  EdgeLabelRenderer,
} from "reactflow";
import { pointsToSVG } from "../utils";

function FlowchartEdge({
  id,
  source,
  target,
  markerEnd,
  style,
  data,
  ...props
}) {
  const sourceNode = useStore((store) => store.nodeInternals.get(source));
  const targetNode = useStore((store) => store.nodeInternals.get(target));

  const allEdges = useStore((store) => store.edges);

  if (!sourceNode || !targetNode) {
    return null;
  }

  var offset = 25;
  let sourcePos, targetPos, edgePath;
  let labelX, labelY;

  // Найти все ребра с таким же источником, как у текущего ребра
  const sx = sourceNode.position.x + sourceNode.width / 2;
  const sy = sourceNode.position.y + sourceNode.height / 2;
  const tx = targetNode.position.x + targetNode.width / 2;
  const ty = targetNode.position.y;

  const sameSourceEdges = allEdges.filter((edge) => edge.source === source);

  if (sameSourceEdges.length === 2) {
    labelY = sy;

    if (tx > sx) {
      sourcePos = "right";
      labelX = sx + sourceNode.width / 2 + offset;
    } else {
      labelX = sx - sourceNode.width / 2 - offset;
      sourcePos = "left";
    }
  } else {
    sourcePos = "bottom";
    labelX = tx + sourceNode.width / 2 + offset;
    labelY = ty - targetNode.height / 2 - offset;
  }

  targetPos = "top";

  [edgePath] = getSmoothStepPath({
    sourceX: sx,
    sourceY: sy,
    sourcePosition: sourcePos,
    targetPosition: targetPos,
    targetX: tx,
    targetY: ty,
    borderRadius: 0,
    offset: offset,
  });

  const labelData = data?.label || [];

  return (
    <>
      <BaseEdge
        path={edgePath}
        markerEnd={markerEnd}
        style={style}
        {...props}
      />
      <EdgeLabelRenderer>
        {labelData.map((label, index) => (
          <div
            key={index}
            style={{
              position: "absolute",
              transform: `translate(-50%, -100%) translate(${labelX}px, ${labelY}px)`,
              pointerEvents: "all",
            }}
          >
            {label}
          </div>
        ))}
      </EdgeLabelRenderer>
    </>
  );
}

export default FlowchartEdge;
