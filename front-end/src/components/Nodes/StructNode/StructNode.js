import React from "react";
import "./StructNode.css";
import { Handle } from "reactflow";

function StructNode({ id, data }) {
  return (
    <div className="struct-node">
      <div className="title">Struct: {data.structName}</div>
      <div className="fields">
        Fields:
        <ul>
          {data.fields.map((field, index) => (
            <li key={index}>{field}</li>
          ))}
        </ul>
      </div>
      <div className="methods">
        Methods:
        <ul>
          {data.methods.map((method, index) => (
            <li key={index}>{method}</li>
          ))}
        </ul>
      </div>
      <Handle type="target" />
      <Handle type="source" />
    </div>
  );
}

export default StructNode;
