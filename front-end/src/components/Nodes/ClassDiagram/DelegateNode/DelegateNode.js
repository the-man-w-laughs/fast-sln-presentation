import React from "react";
import CircleWithLetter from "../Utils/CircleWithLetter/CircleWithLetter";
import { Handle } from "reactflow";
import Divider from "../Utils/Divider/Divider";
import GenericInfo from "../Utils/GenericInfo/GenericInfo";
import "./DelegateNode.css";

function DelegateNode({ id, data }) {
  return (
    <div className="delegate-node node">
      <div className="title-container">
        <CircleWithLetter letter="D" />
        <div className="title">{data.name}</div>
      </div>
      {data.genericInfo?.length > 0 && <GenericInfo info={data.genericInfo} />}
      <Divider></Divider>
      <div className="return-type">{data.returnType}</div>
      <Divider></Divider>
      <div className="parameters">
        <table>
          <tbody>
            {data.parameters.map((parameter, index) => (
              <tr key={index}>
                <td>{parameter}</td>
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

export default DelegateNode;
