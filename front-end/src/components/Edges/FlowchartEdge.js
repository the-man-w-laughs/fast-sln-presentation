import { useCallback } from "react";
import { useStore, BaseEdge, getSmoothStepPath } from "reactflow";

/**
 * Converts an array of points to an SVG path string.
 * @param {Array} points - Array of points with x and y coordinates.
 * @returns {string} SVG path string.
 */
function pointsToSVG(points) {
  let svgPath = "M" + points[0].x + "," + points[0].y;
  for (let i = 1; i < points.length; i++) {
    svgPath += " L" + points[i].x + "," + points[i].y;
  }
  return svgPath;
}

/**
 * Custom edge component for flowchart edges.
 * @param {Object} props - Component props.
 * @returns {JSX.Element|null} JSX element representing the flowchart edge.
 */
function FlowchartEdge({ id, source, target, markerEnd, style, ...props }) {
  // Get all nodes from the store
  const allNodes = useStore((store) =>
    Array.from(store.nodeInternals.values())
  );

  // Get source and target nodes from the store
  const sourceNode = useStore((store) => store.nodeInternals.get(source));
  const targetNode = useStore((store) => store.nodeInternals.get(target));

  // If source or target node is not found, return null
  if (!sourceNode || !targetNode) {
    return null;
  }

  // Offset for smooth step path
  var offset = 40;

  // Calculate source and target coordinates
  const sx = sourceNode.position.x + sourceNode.width / 2;
  const sy = sourceNode.position.y + sourceNode.height;
  const tx = targetNode.position.x + targetNode.width / 2;
  const ty = targetNode.position.y;

  let sourcePos, targetPos, edgePath;

  // Determine source and target positions
  if (sy < ty) {
    sourcePos = "bottom";
    targetPos = "top";

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
    var minX = Math.min(sx, tx);

    // Find the minimum X-coordinate of intermediate nodes
    for (let i = 1; i < allNodes.length; i++) {
      if (allNodes[i].position.y < sy && allNodes[i].position.y > ty) {
        if (allNodes[i].position.x < minX) {
          minX = allNodes[i].position.x;
        }
      }
    }

    // Create SVG path for left to left edges
    edgePath = pointsToSVG([
      { x: sx, y: sy - sourceNode.height / 2 },
      { x: minX - offset, y: sy - sourceNode.height / 2 },
      { x: minX - offset, y: ty + targetNode.height / 2 },
      { x: tx - targetNode.width / 2, y: ty + targetNode.height / 2 },
    ]);
  }

  // Render the edge component with calculated path
  return (
    <BaseEdge path={edgePath} markerEnd={markerEnd} style={style} {...props} />
  );
}

export default FlowchartEdge;
