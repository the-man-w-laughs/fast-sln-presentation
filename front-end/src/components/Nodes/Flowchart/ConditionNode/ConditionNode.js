import React, { useEffect, useRef, getBoundingClientRect } from "react";
import { Handle, Position } from "reactflow";
import "./ConditionNode.css";

function ConditionNode({ id, data }) {
  const diamondRef = useRef(null);
  const conditionRef = useRef(null);

  useEffect(() => {
    if (diamondRef.current && conditionRef.current) {
      const contentCondition = document.querySelector(".content-condition");
      const contentRect = contentCondition.getBoundingClientRect();
      const size = Math.max(contentRect.width, contentRect.height);
      diamondRef.current.style.width = `${size}px`;
      diamondRef.current.style.height = `${size}px`;
      const diamondSize = diamondRef.current.getBoundingClientRect();
      console.log(diamondSize);
      conditionRef.current.style.width = `${diamondSize.width}px`;
      conditionRef.current.style.height = `${diamondSize.height}px`;
    }
  }, [data.content]);

  return (
    <div ref={conditionRef} className="condition">
      <div ref={diamondRef} className="diamond"></div>
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
