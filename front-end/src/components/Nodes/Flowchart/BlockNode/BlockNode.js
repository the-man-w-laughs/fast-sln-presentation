import React from "react";
import { Handle, Position } from "reactflow";
import "./BlockNode.css";

function BlockNode({ id, data }) {
  return (
    <div className="block-node node">
      <div className="content">
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

export default BlockNode;
