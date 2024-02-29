import React from "react";
import "./RecordNode.css";
import { Handle } from "reactflow";

function RecordNode({ id, data }) {
  return (
    <div className="record-node">
      <div className="title">Record: {data.recordName}</div>
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

export default RecordNode;
