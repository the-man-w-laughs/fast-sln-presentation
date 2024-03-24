import React, { useEffect, useRef } from "react";
import { Handle, Position } from "reactflow";
import "./ConditionNode.css";

function ConditionNode({ id, data }) {
  const figureRef = useRef(null);
  const conditionRef = useRef(null);

  useEffect(() => {
    if (figureRef.current && conditionRef.current) {
      const contentCondition = document.querySelector(".content-condition");
      const contentRect = contentCondition.getBoundingClientRect();
      const size = Math.min(contentRect.height, contentRect.width);
      figureRef.current.style.width = `${size}px`;
      figureRef.current.style.height = `${size}px`;
      conditionRef.current.style.width = `${size}px`;
      conditionRef.current.style.height = `${size}px`;
    }
  }, [data.content]);

  return (
    <div ref={conditionRef} className="condition-node">
      <svg ref={figureRef} viewBox="0 0 100 100" className="rhombus">
        <polygon points="50,0 100,50 50,100 0,50" />
      </svg>
      <div className="content-condition">
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

export default ConditionNode;
