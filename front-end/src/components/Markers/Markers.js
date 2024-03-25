import React from "react";

const Markers = () => (
  <svg style={{ position: "absolute" }}>
    <defs>
      <marker
        id="TriangleUnfilled"
        viewBox="0 0 40 40"
        markerHeight={20}
        markerWidth={20}
        refX={40}
        refY={20}
        orient="auto"
      >
        <path
          d="M0,0 L0,40 L40,20 Z"
          fill="#FFFFFF"
          stroke="#000000"
          strokeWidth="2"
        />
      </marker>

      <marker
        id="TriangleFilled"
        viewBox="0 0 40 40"
        markerHeight={20}
        markerWidth={20}
        refX={40}
        refY={20}
        orient="auto"
      >
        <path
          d="M0,0 L0,40 L40,20 Z"
          fill="#000000"
          stroke="#000000"
          strokeWidth="2"
        />
      </marker>

      <marker
        id="RhombusUnfilled"
        viewBox="0 0 80 40"
        markerHeight={40}
        markerWidth={40}
        refX={80}
        refY={20}
        orient="auto"
      >
        <path
          d="M0,20 L40,40 L80,20 L40,0 Z"
          fill="#FFFFFF"
          stroke="#000000"
          strokeWidth="2"
        />
      </marker>

      <marker
        id="RhombusFilled"
        viewBox="0 0 80 40"
        markerHeight={40}
        markerWidth={40}
        refX={80}
        refY={20}
        orient="auto"
      >
        <path
          d="M0,20 L40,40 L80,20 L40,0 Z"
          fill="#000000"
          stroke="#000000"
          strokeWidth="2"
        />
      </marker>

      <marker
        id="Arrow"
        viewBox="0 0 40 40"
        markerHeight={20}
        markerWidth={20}
        refX={40}
        refY={20}
        orient="auto"
      >
        <path
          d="M0,40 L40,20 L0,0 M40,20 L0,20"
          fill="#FFFFFF"
          stroke="#000000"
          strokeWidth="2"
        />
      </marker>
    </defs>
  </svg>
);

export default Markers;
