import React from "react";

const TriangleUnfilledArrow = () => (
  <svg style={{ position: "absolute" }}>
    <defs>
      <marker
        id="TriangleUnfilledArrow"
        viewBox="0 0 40 40"
        markerHeight={20}
        markerWidth={20}
        refX={20}
        refY={20}
        orient="auto"
      >
        <path
          d="M0,0 L0,40 L20,20 L0,0"
          fill="#FFFFFF"
          stroke="#1A192B"
          strokeWidth="2"
        />
      </marker>
    </defs>
  </svg>
);

export default TriangleUnfilledArrow;
