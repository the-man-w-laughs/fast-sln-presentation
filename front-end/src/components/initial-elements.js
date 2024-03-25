import React from "react";
import { MarkerType, Position } from "reactflow";

export const initialNodes = [
  {
    id: "terminalNode1",
    type: "terminalNode",
    data: { content: ["Начало Main"] },
  },
  {
    id: "blockNode1",
    type: "blockNode",
    data: { content: ["var s = string.Empty;"] },
  },
  {
    id: "conditionNode1",
    type: "conditionNode",
    data: { content: ["s"] },
  },
  {
    id: "blockNode2",
    type: "blockNode",
    data: { content: ['Console.WriteLine("да")'] },
  },
  {
    id: "blockNode3",
    type: "blockNode",
    data: { content: ['Console.WriteLine("default")'] },
  },
  {
    id: "terminalNode2",
    type: "terminalNode",
    data: { content: ["Конец Main"] },
  },
];
export const initialEdges = [
  {
    id: "terminalNode1-blockNode1",
    source: "terminalNode1",
    target: "blockNode1",
    type: "arrow",
    data: { label: [] },
  },
  {
    id: "blockNode1-conditionNode1",
    source: "blockNode1",
    target: "conditionNode1",
    type: "arrow",
    data: { label: [] },
  },
  {
    id: "conditionNode1-blockNode2",
    source: "conditionNode1",
    target: "blockNode2",
    type: "arrow",
    data: { label: ['"да"', '"zalupa"'] },
  },
  {
    id: "conditionNode1-blockNode3",
    source: "conditionNode1",
    target: "blockNode3",
    type: "arrow",
    data: { label: ["default"] },
  },
  {
    id: "blockNode2-terminalNode2",
    source: "blockNode2",
    target: "terminalNode2",
    type: "arrow",
    data: { label: [] },
  },
  {
    id: "blockNode3-terminalNode2",
    source: "blockNode3",
    target: "terminalNode2",
    type: "arrow",
    data: { label: [] },
  },
];
