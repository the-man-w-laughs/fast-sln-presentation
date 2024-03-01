import React from "react";
import { Handle } from "reactflow";
import CircleWithLetter from "../Utils/CircleWithLetter/CircleWithLetter";
import GenericInfo from "../Utils/GenericInfo/GenericInfo";
import Divider from "../Utils/Divider/Divider";
import "./RecordNode.css";

function RecordNode({ id, data }) {
  return (
    <div className="record-node node">
      <div className="title-container">
        <CircleWithLetter letter="R" />
        <div className="title">{data.recordName}</div>
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

export default RecordNode;
