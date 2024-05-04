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
  const allNodes = useStore((store) =>
    Array.from(store.nodeInternals.values())
  );

  if (!sourceNode || !targetNode) {
    return null;
  }

  var offset = 25;
  let sourcePos, targetPos, edgePath;
  let labelX, labelY;

  if (sourceNode.position.y < targetNode.position.y) {
    sourcePos = "bottom";
    targetPos = "top";

    const sx = sourceNode.position.x + sourceNode.width / 2;
    const sy = sourceNode.position.y + sourceNode.height;
    const tx = targetNode.position.x + targetNode.width / 2;
    const ty = targetNode.position.y;

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

    labelX = (sx + tx) / 2;
    labelY = (sy + ty) / 2;
  } else {
    sourcePos = "left";
    targetPos = "left";
    var minX = Math.min(sourceNode.position.y, targetNode.position.y);

    for (let i = 1; i < allNodes.length; i++) {
      if (
        allNodes[i].position.y < sourceNode.position.y &&
        allNodes[i].position.y > targetNode.position.y
      ) {
        if (allNodes[i].position.x < minX) {
          minX = allNodes[i].position.x;
        }
      }
    }

    edgePath = pointsToSVG([
      {
        x: sourceNode.position.x,
        y: sourceNode.position.y + sourceNode.height / 2,
      },
      { x: minX - offset, y: sourceNode.position.y + sourceNode.height / 2 },
      { x: minX - offset, y: targetNode.position.y - offset },
      {
        x: targetNode.position.x + targetNode.width / 2,
        y: targetNode.position.y - offset,
      },
    ]);

    labelX = (sourceNode.position.x + minX) / 2;
    labelY = sourceNode.position.y + sourceNode.height / 2;
  }

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
        <div
          style={{
            position: "absolute",
            transform: `translate(-50%, -100%) translate(${labelX}px,${labelY}px)`,
            pointerEvents: "all",
          }}
        >
          {labelData.map((label, index) => (
            <div key={index}>{label}</div>
          ))}
        </div>
      </EdgeLabelRenderer>
    </>
  );
}

export default FlowchartEdge;
