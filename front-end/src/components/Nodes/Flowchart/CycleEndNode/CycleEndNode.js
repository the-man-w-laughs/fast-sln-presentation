import React, { useEffect, useRef } from "react";
import { Handle, Position } from "reactflow";
import "./CycleEndNode.css";

function CycleEndNode({ id, data }) {
  const figureRef = useRef(null);
  const cycleStartRef = useRef(null);

  useEffect(() => {
    if (figureRef.current && cycleStartRef.current) {
      const contentCondition = document.querySelector(".content-cycleEnd");
      const contentRect = contentCondition.getBoundingClientRect();
      figureRef.current.style.width = `${contentRect.width}px`;
      figureRef.current.style.height = `${contentRect.height}px`;
      cycleStartRef.current.style.width = `${contentRect.width}px`;
      cycleStartRef.current.style.height = `${contentRect.height}px`;
    }
  }, [data.content]);

  return (
    <div ref={cycleStartRef} className="cycleEnd-node">
      <svg ref={figureRef} viewBox="0 0 100 100" className="cycleEnd">
        <polygon points="0,0 0,80 20,100 80,100 100,80 100,0" />
      </svg>
      <div className="content-cycleEnd">
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
