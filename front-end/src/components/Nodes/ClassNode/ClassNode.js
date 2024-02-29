import React from "react";
import "./ClassNode.css";
import { Handle } from "reactflow";

function ClassNode({ id, data }) {
  return (
    <div className="ClassNode">
      <div className="title">Class: {data.className}</div>
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

export default ClassNode;
