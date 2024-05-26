import React from "react";
import { Handle } from "reactflow";
import CircleWithLetter from "../Utils/CircleWithLetter/CircleWithLetter";
import Divider from "../Utils/Divider/Divider";
import GenericInfo from "../Utils/GenericInfo/GenericInfo";
import "./StructNode.css";

function StructNode({ id, data }) {
  return (
    <div className="struct-node node">
      <div className="title-container">
        <CircleWithLetter letter="S" />
        <div className="title">{data.name}</div>
      </div>
      {data.genericInfo?.length > 0 && <GenericInfo info={data.genericInfo} />}

      {data.showContent && (
        <>
          <Divider />
          <div className="members">
            <table>
              <tbody>
                {data.members.map((member, index) => (
                  <tr key={index}>
                    <td>{member}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          <Divider />
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
        </>
      )}
      <Handle type="target" />
      <Handle type="source" />
    </div>
  );
}

export default StructNode;
