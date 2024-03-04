import React from "react";
import { Handle } from "reactflow";
import CircleWithLetter from "../Utils/CircleWithLetter/CircleWithLetter";
import Divider from "../Utils/Divider/Divider";
import GenericInfo from "../Utils/GenericInfo/GenericInfo";
import "./InterfaceNode.css";

function InterfaceNode({ id, data }) {
  return (
    <div className="interface-node node">
      <div className="title-container">
        <CircleWithLetter letter="I" />
        <div className="title">{data.name}</div>
      </div>
      {data.genericInfo?.length > 0 && <GenericInfo info={data.genericInfo} />}
      <Divider></Divider>
      <div className="methods">
        <table>
          <tbody>
            {data.methods.map((method, index) => (
              <tr key={index}>
                <td>{method};</td>
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

export default InterfaceNode;
