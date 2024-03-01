import React from "react";
import { MarkerType, Position } from "reactflow";

export const initialNodes = [
  {
    id: "1",
    type: "classNode",
    className: "classNode",
    data: {
      className: "MyClass",
      fields: ["field1", "field2", "field3"],
      methods: ["method1()", "method2()", "method3()"],
      genericInfo: "zalupa",
    },
    draggable: true,
    selectable: false,
    position: { x: 0, y: 0 },
  },
  {
    id: "2",
    type: "interfaceNode",
    className: "interfaceNode",
    data: {
      interfaceName: "MyInterface",
      methods: ["method1()", "method2()", "method3()"],
      genericInfo: "zalupa",
    },
    draggable: true,
    selectable: false,
    position: { x: 400, y: 0 },
  },
  {
    id: "3",
    type: "structNode",
    className: "structNode",
    data: {
      structName: "MyStruct",
      fields: ["field1", "field2", "field3"],
      methods: ["method1()", "method2()", "method3()"],
      genericInfo: "zalupa",
    },
    draggable: true,
    selectable: false,
    position: { x: 0, y: 400 },
  },
  {
    id: "4",
    type: "recordNode",
    className: "recordNode",
    data: {
      recordName: "MyRecord",
      fields: ["field1", "field2", "field3"],
      methods: ["method1()", "method2()", "method3()"],
      genericInfo: "zalupa",
    },
    draggable: true,
    selectable: false,
    position: { x: 800, y: 0 },
  },
  {
    id: "5",
    type: "enumNode",
    className: "enumNode",
    data: {
      enumName: "MyEnum",
      values: ["Value1", "Value2", "Value3"],
      genericInfo: "zalupa",
    },
    draggable: true,
    selectable: false,
    position: { x: 400, y: 400 },
  },
  {
    id: "6",
    type: "delegateNode",
    className: "delegateNode",
    data: {
      delegateName: "MyDelegate",
      returnType: "void",
      parameters: ["param1: int", "param2: string"],
      genericInfo: "zalupa",
    },
    draggable: true,
    selectable: false,
    position: { x: 500, y: 500 },
  },
];

export const initialEdges = [
  {
    id: `edge-${1}`,
    target: "1",
    source: `2`,
    type: "floating",
    markerEnd: {
      type: MarkerType.Arrow,
      color: "#FF0072",
    },
    style: {
      strokeWidth: 2,
      strokeDasharray: [5, 5],
      stroke: "#FF0072",
    },
  },
  // { id: "e1-3", source: "2", target: "3", animated: true },
  // {
  //   id: "e4-5",
  //   source: "4",
  //   target: "5",
  //   type: "smoothstep",
  //   sourceHandle: "handle-0",
  //   data: {
  //     selectIndex: 0,
  //   },
  //   markerEnd: {
  //     type: MarkerType.ArrowClosed,
  //   },
  // },
  // {
  //   id: "e4-6",
  //   source: "4",
  //   target: "6",
  //   type: "smoothstep",
  //   sourceHandle: "handle-1",
  //   data: {
  //     selectIndex: 1,
  //   },
  //   markerEnd: {
  //     type: MarkerType.ArrowClosed,
  //   },
  // },
];
