import React from "react";
import CircleWithLetter from "../Utils/CircleWithLetter/CircleWithLetter";
import "./EnumNode.css";
import Divider from "../Utils/Divider/Divider";
import { Handle } from "reactflow";

function EnumNode({ id, data }) {
  return (
    <div className="enum-node node">
      <div className="title-container">
        <CircleWithLetter letter="E" />
        <div className="title">{data.name}</div>
      </div>
      {data.showContent && (
        <>
          <Divider />
          <div className="values">
            <table>
              <tbody>
                {data.members.map((member, index) => (
                  <tr key={index}>
                    <td>{member},</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </>
      )}
      <Handle type="target" />
      <Handle type="source" />
    </div>
  );
}

export default EnumNode;
