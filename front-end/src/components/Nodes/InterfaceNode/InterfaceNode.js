// InterfaceNode.js

import React from "react";
import "./InterfaceNode.css";

function InterfaceNode({ id, data }) {
  return (
    <div className="InterfaceNode">
      <div className="title">Interface: {data.interfaceName}</div>
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

export default InterfaceNode;
