import React from "react";
import "./EnumNode.css";

function EnumNode({ id, data }) {
  return (
    <div className="enum-node">
      <div className="title">Enum: {data.enumName}</div>
      <div className="values">
        Values:
        <ul>
          {data.values.map((value, index) => (
            <li key={index}>{value}</li>
          ))}
        </ul>
      </div>
    </div>
  );
}

export default EnumNode;
