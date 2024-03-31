import React, { useEffect, useRef } from "react";
import { Handle, Position } from "reactflow";
import "./ConditionNode.css";

function ConditionNode({ id, data }) {
  const rhombusRef = useRef(null);
  const contentRef = useRef(null);

  useEffect(() => {
    if (rhombusRef.current && contentRef.current) {
      const size = contentRef.current.getBoundingClientRect();
      const maxSize = Math.max(size.width, size.height) + 5;

      rhombusRef.current.style.width = `${maxSize}px`;
      rhombusRef.current.style.height = `${maxSize}px`;
    }
  }, []);
  return (
    <div className="condition-node">
      <svg
        ref={rhombusRef}
        viewBox="0 0 100 100"
        className="rhombus"
        width={100}
        height={100}
      >
        <polygon points="50,0 100,50 50,100 0,50" />
      </svg>
      <div ref={contentRef} className="content-condition">
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
