import React, { useEffect, useRef } from "react";
import { Handle, Position } from "reactflow";
import "./CycleStartNode.css";

function CycleStartNode({ id, data }) {
  const contentCycleStartRef = useRef(null);
  const figureRef = useRef(null);
  const cycleStartRef = useRef(null);

  useEffect(() => {
    if (figureRef.current && cycleStartRef.current) {
      const contentRect = contentCycleStartRef.current.getBoundingClientRect();
      var maxValue = Math.max(contentRect.width, contentRect.height);
      figureRef.current.style.width = `${maxValue}px`;
      figureRef.current.style.height = `${maxValue}px`;
      cycleStartRef.current.style.width = `${maxValue}px`;
      cycleStartRef.current.style.height = `${maxValue}px`;
    }
  }, [data.content]);

  return (
    <div ref={cycleStartRef} className="cycleStart-node">
      <svg ref={figureRef} viewBox="0 0 100 100" className="cycleStart">
        <polygon points="0,100 0,20 20,0 80,0 100,20 100,100" />
      </svg>
      <div ref={contentCycleStartRef} className="content-cycleStart">
        <table>
          <tbody>
            {data.content.map((member, index) => (
              <tr key={index}>
                <td>{member}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <Handle type="target" position={Position.Top} />
      <Handle type="source" position={Position.Bottom} />
    </div>
  );
}

export default CycleStartNode;
