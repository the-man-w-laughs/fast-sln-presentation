import React from "react";
import CircleWithLetter from "../Utils/CircleWithLetter/CircleWithLetter";
import "./EnumNode.css";
import Divider from "../Utils/Divider/Divider";

function EnumNode({ id, data }) {
  return (
    <div className="enum-node node">
      <div className="title-container">
        <CircleWithLetter letter="E" />
        <div className="title">{data.enumName}</div>
      </div>
      <Divider />
      <div className="values">
        <table>
          <tbody>
            {data.values.map((value, index) => (
              <tr key={index}>
                <td>{value},</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default EnumNode;
