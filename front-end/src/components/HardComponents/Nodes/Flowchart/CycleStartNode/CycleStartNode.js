import React, { useEffect, useState } from "react";
import { Handle, Position } from "reactflow";
import "./CycleStartNode.css";

function CycleStartNode({ id, data }) {
  const [contentSize, setContentSize] = useState({ width: 0, height: 0 });

  useEffect(() => {
    const contentElement = document.getElementById(`content-cycleStart-${id}`);
    if (contentElement) {
      const { offsetWidth, offsetHeight } = contentElement;
      const maxSize = Math.max(offsetHeight, offsetWidth);
      setContentSize({ width: maxSize, height: maxSize });
    }
  }, [id, data]);

  return (
    <div className="cycleStart-node">
      <svg
        id={`cycleStart-${id}`}
        viewBox="0 0 100 100"
        className="cycleStart"
        width={contentSize.width}
        height={contentSize.height}
      >
        <polygon points="0,100 0,20 20,0 80,0 100,20 100,100" />
      </svg>
      <div id={`content-cycleStart-${id}`} className="content-cycleStart">
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
