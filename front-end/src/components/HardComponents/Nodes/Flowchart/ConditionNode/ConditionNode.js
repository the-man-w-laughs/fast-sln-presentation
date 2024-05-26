import React, { useEffect, useRef, useState } from "react";
import { Handle, Position } from "reactflow";
import "./ConditionNode.css";

function ConditionNode({ id, data }) {
  const [contentSize, setContentSize] = useState({ width: 0, height: 0 });

  useEffect(() => {
    const contentElement = document.getElementById(`content-condition-${id}`);
    if (contentElement) {
      const { offsetWidth, offsetHeight } = contentElement;
      const maxSize = Math.max(offsetHeight, offsetWidth) + 10;
      setContentSize({ width: maxSize, height: maxSize });
    }
  }, [id, data]);
  return (
    <div className="condition-node">
      <svg
        id={`rhombus-${id}`}
        viewBox="0 0 100 100"
        className="rhombus"
        width={contentSize.width}
        height={contentSize.height}
      >
        <polygon points="50,0 100,50 50,100 0,50" />
      </svg>
      <div id={`content-condition-${id}`} className="content-condition">
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
      <Handle type="source" position={Position.Left} />
      <Handle type="source" position={Position.Right} />
    </div>
  );
}

export default ConditionNode;
