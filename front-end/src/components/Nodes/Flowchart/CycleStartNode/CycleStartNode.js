import React, { useEffect, useRef } from "react";
import { Handle, Position } from "reactflow";
import "./CycleStartNode.css";

function CycleStartNode({ id, data }) {
  const svgRef = useRef(null);

  useEffect(() => {
    const svg = svgRef.current;
    if (svg) {
      const bbox = svg.getBBox();
      svg.setAttribute("width", bbox.width);
      svg.setAttribute("height", bbox.height);
    }
  }, []);

  return (
    <div className="cycleStart-node">
      <svg ref={svgRef} className="border-svg">
        <rect />
        <text x="50%" y="50%" dominantBaseline="middle" textAnchor="middle">
          {data.content.join("\n")}
        </text>
      </svg>
      <Handle type="target" position={Position.Top} />
      <Handle type="source" position={Position.Bottom} />
    </div>
  );
}

export default CycleStartNode;
