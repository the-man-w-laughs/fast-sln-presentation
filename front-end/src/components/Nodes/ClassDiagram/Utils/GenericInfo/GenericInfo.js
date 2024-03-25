import React from "react";
import "./GenericInfo.css";

function GenericInfo({ info }) {
  return (
    <div className="generic-info-container">
      <div className="generic-info">
        {info.map((item, index) => (
          <div key={index}>
            <span>{item},</span>
          </div>
        ))}
      </div>
    </div>
  );
}

export default GenericInfo;
