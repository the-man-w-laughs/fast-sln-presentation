import React from "react";
import { MarkerType, Position } from "reactflow";

export const initialNodes = [
  {
    id: "block-1",
    type: "blockNode",
    data: {
      content: ["var a = 1;", "var b = 2;"],
    },
  },
  {
    id: "block-2",
    type: "blockNode",
    data: {
      content: ["var c = 3;", "var d = 4;"],
    },
  },
];
export const initialEdges = [
  {
    id: "0",
    source: "block-1",
    target: "block-2",
    type: "arrow",
  },
];
