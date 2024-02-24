import React from "react";
import "./StructNode.css";

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
    </div>
  );
}

export default StructNode;
