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

export default ClassNode;
