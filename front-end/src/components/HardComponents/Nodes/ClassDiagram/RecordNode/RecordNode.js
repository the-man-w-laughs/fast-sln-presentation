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
        <div className="title">{data.name}</div>
      </div>
      {data.genericInfo?.length > 0 && <GenericInfo info={data.genericInfo} />}

      {data.showContent && (
        <>
          <Divider></Divider>
          <div className="members">
            <table>
              <tbody>
                {data.members.map((member, index) => (
                  <tr key={index}>
                    <td>{member};</td>
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
                    <td>{method};</td>
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

export default RecordNode;
