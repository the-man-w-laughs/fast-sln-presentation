import React from "react";
import "./GenericInfo.css";

function GenericInfo({ info }) {
  return (
    <div className="generic-info-container">
      <div className="generic-info">
        <span>{info}</span>
      </div>
    </div>
  );
}

export default GenericInfo;
