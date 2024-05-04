import React, { useEffect, useRef, useState } from "react";
import { Handle, Position } from "reactflow";
import "./CycleEndNode.css";

function CycleEndNode({ id, data }) {
  const [contentSize, setContentSize] = useState({ width: 0, height: 0 });

  useEffect(() => {
    const contentElement = document.getElementById(`content-cycleEnd-${id}`);
    if (contentElement) {
      const { offsetWidth, offsetHeight } = contentElement;
      const maxSize = Math.max(offsetHeight, offsetWidth);
      setContentSize({ width: maxSize, height: maxSize });
    }
  }, [id, data]);

  return (
    <div className="cycleEnd-node">
      <svg
        id={`cycleEnd-${id}`}
        viewBox="0 0 100 100"
        className="cycleEnd"
        width={contentSize.width}
        height={contentSize.height}
      >
        <polygon points="0,0 0,80 20,100 80,100 100,80 100,0" />
      </svg>
      <div id={`content-cycleEnd-${id}`} className="content-cycleEnd">
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
