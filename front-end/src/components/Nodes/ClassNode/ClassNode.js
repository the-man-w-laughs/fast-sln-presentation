import React from "react";
import { Handle } from "reactflow";
import CircleWithLetter from "../Utils/CircleWithLetter/CircleWithLetter";
import Divider from "../Utils/Divider/Divider";
import GenericInfo from "../Utils/GenericInfo/GenericInfo";
import "./ClassNode.css";

function ClassNode({ id, data }) {
  return (
    <div className="class-node node">
      <div className="title-container">
        <CircleWithLetter letter="C" />
        <div className="title">{data.className}</div>
      </div>
      {data.genericInfo && <GenericInfo info={data.genericInfo} />}
      <Divider></Divider>
      <div className="fields">
        <table>
          <tbody>
            {data.fields.map((field, index) => (
              <tr key={index}>
                <td>{field}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <Divider></Divider>
      <div className="methods">
        <table>
          <tbody>
            {data.methods.map((method, index) => (
              <tr key={index}>
                <td>{method}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      <Handle type="target" />
      <Handle type="source" />
    </div>
  );
}

export default ClassNode;
