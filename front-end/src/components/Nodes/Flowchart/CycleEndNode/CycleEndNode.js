import React, { useEffect, useRef } from "react";
import { Handle, Position } from "reactflow";
import "./CycleEndNode.css";

function CycleEndNode({ id, data }) {
  const contentCycleEndRef = useRef(null);
  const figureRef = useRef(null);
  const cycleStartRef = useRef(null);

  useEffect(() => {
    if (
      figureRef.current &&
      cycleStartRef.current &&
      contentCycleEndRef.current
    ) {
      const contentRect = contentCycleEndRef.current.getBoundingClientRect();
      var maxValue = Math.max(contentRect.width, contentRect.height);
      figureRef.current.style.width = `${maxValue}px`;
      figureRef.current.style.height = `${maxValue}px`;
      cycleStartRef.current.style.width = `${maxValue}px`;
      cycleStartRef.current.style.height = `${maxValue}px`;
    }
  }, [data.content]);

  return (
    <div ref={cycleStartRef} className="cycleEnd-node">
      <svg ref={figureRef} viewBox="0 0 100 100" className="cycleEnd">
        <polygon points="0,0 0,80 20,100 80,100 100,80 100,0" />
      </svg>
      <div ref={contentCycleEndRef} className="content-cycleEnd">
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

export default CycleEndNode;
