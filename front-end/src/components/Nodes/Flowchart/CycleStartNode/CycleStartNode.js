import React, { useEffect, useRef } from "react";
import { Handle, Position } from "reactflow";
import "./CycleStartNode.css";

function CycleStartNode({ id, data }) {
  const figureRef = useRef(null);
  const cycleStartRef = useRef(null);

  useEffect(() => {
    if (figureRef.current && cycleStartRef.current) {
      const contentCondition = document.querySelector(".content-cycleStart");
      const contentRect = contentCondition.getBoundingClientRect();
      figureRef.current.style.width = `${contentRect.width}px`;
      figureRef.current.style.height = `${contentRect.height}px`;
      cycleStartRef.current.style.width = `${contentRect.width}px`;
      cycleStartRef.current.style.height = `${contentRect.height}px`;
    }
  }, [data.content]);

  return (
    <div ref={cycleStartRef} className="cycleStart-node">
      <svg ref={figureRef} viewBox="0 0 100 100" className="cycleStart">
        <polygon points="0,100 0,20 20,0 80,0 100,20 100,100" />
      </svg>
      <div className="content-cycleStart">
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
