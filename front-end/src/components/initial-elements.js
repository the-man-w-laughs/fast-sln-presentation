import React from "react";
import { MarkerType, Position } from "reactflow";

export const initialNodes = [
  {
    id: "block-0",
    type: "terminalNode",
    data: {
      content: ["Начало"],
    },
  },
  {
    id: "block-1",
    type: "blockNode",
    data: {
      content: ["var a = 1;", "var b = 2;"],
    },
  },
  {
    id: "block-2",
    type: "cycleStartNode",
    data: {
      content: ["var c = 3;", "var d = 4;"],
    },
  },
  {
    id: "block-3",
    type: "blockNode",
    data: {
      content: ["var c = 3;", "var d = 4;"],
    },
  },
  {
    id: "block-4",
    type: "conditionNode",
    data: {
      content: ["a > b"],
    },
  },
  {
    id: "block-5",
    type: "blockNode",
    data: {
      content: ["var c = 3;", "var d = 4;"],
    },
  },
  {
    id: "block-6",
    type: "blockNode",
    data: {
      content: ["var c = 3;", "var d = 4;"],
    },
  },
  {
    id: "block-7",
    type: "terminalNode",
    data: {
      content: ["Конец"],
    },
  },
  {
    id: "block-8",
    type: "cycleEndNode",
    data: {
      content: ["var c = 3;", "var d = 4;"],
    },
  },
];
export const initialEdges = [
  {
    id: "1",
    source: "block-1",
    target: "block-2",
    type: "arrow",
  },
  {
    id: "2",
    source: "block-1",
    target: "block-3",
    type: "arrow",
  },
  {
    id: "3",
    source: "block-8",
    target: "block-4",
    type: "arrow",
  },
  {
    id: "4",
    source: "block-3",
    target: "block-4",
    type: "arrow",
  },
  {
    id: "5",
    source: "block-4",
    target: "block-1",
    type: "arrow",
  },
  {
    id: "6",
    source: "block-4",
    target: "block-5",
    type: "arrow",
  },
  {
    id: "7",
    source: "block-4",
    target: "block-6",
    type: "arrow",
  },
  {
    id: "8",
    source: "block-6",
    target: "block-7",
    type: "arrow",
  },
  {
    id: "9",
    source: "block-5",
    target: "block-7",
    type: "arrow",
  },
  {
    id: "10",
    source: "block-0",
    target: "block-1",
    type: "arrow",
  },
  {
    id: "11",
    source: "block-2",
    target: "block-8",
    type: "arrow",
  },
];
