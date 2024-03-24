import { useCallback } from "react";
import { useStore, BaseEdge, getSmoothStepPath } from "reactflow";
import { pointsToSVG } from "../utils";

/**
 * Custom edge component for flowchart edges.
 * @param {Object} props - Component props.
 * @returns {JSX.Element|null} JSX element representing the flowchart edge.
 */
function FlowchartEdge({ id, source, target, markerEnd, style, ...props }) {
  // Get source and target nodes from the store
  const sourceNode = useStore((store) => store.nodeInternals.get(source));
  const targetNode = useStore((store) => store.nodeInternals.get(target));
  // Get all nodes from the store
  const allNodes = useStore((store) =>
    Array.from(store.nodeInternals.values())
  );

  // If source or target node is not found, return null
  if (!sourceNode || !targetNode) {
    return null;
  }

  // Offset for smooth step path
  var offset = 40;

  let sourcePos, targetPos, edgePath;

  // Determine source and target positions
  if (sourceNode.position.y < targetNode.position.y) {
    sourcePos = "bottom";
    targetPos = "top";

    // Calculate source and target coordinates
    const sx = sourceNode.position.x + sourceNode.width / 2;
    const sy = sourceNode.position.y + sourceNode.height;
    const tx = targetNode.position.x + targetNode.width / 2;
    const ty = targetNode.position.y;

    // Calculate smooth step path for top to bottom edges
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
  } else {
    sourcePos = "left";
    targetPos = "left";
    var minX = Math.min(sourceNode.position.y, targetNode.position.y);

    // Find the minimum X-coordinate of intermediate nodes
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

    // Create SVG path for left to left edges
    edgePath = pointsToSVG([
      // out
      {
        x: sourceNode.position.x,
        y: sourceNode.position.y + sourceNode.height / 2,
      },
      // left to top
      { x: minX - offset, y: sourceNode.position.y + sourceNode.height / 2 },
      // top to right
      { x: minX - offset, y: targetNode.position.y - offset },
      // in
      {
        x: targetNode.position.x + targetNode.width / 2,
        y: targetNode.position.y - offset,
      },
    ]);
  }

  // Render the edge component with calculated path
  return (
    <BaseEdge path={edgePath} markerEnd={markerEnd} style={style} {...props} />
  );
}

export default FlowchartEdge;
