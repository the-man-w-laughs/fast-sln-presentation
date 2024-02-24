import React from "react";
import "./RecordNode.css";

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
    </div>
  );
}

export default RecordNode;
