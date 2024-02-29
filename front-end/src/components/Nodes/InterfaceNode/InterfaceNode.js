// InterfaceNode.js

import React from "react";
import "./InterfaceNode.css";
import { Handle } from "reactflow";

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
      <Handle type="target" />
      <Handle type="source" />
    </div>
  );
}

export default InterfaceNode;
