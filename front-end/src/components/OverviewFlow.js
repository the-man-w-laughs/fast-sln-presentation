import React, { useCallback } from "react";
import ReactFlow, {
  addEdge,
  MiniMap,
  Controls,
  Background,
  useNodesState,
  useEdgesState,
  Panel,
} from "reactflow";

import {
  nodes as initialNodes,
  edges as initialEdges,
} from "./initial-elements";

import "reactflow/dist/style.css";
import "./overview.css";
import ClassNode from "./Nodes/ClassNode/ClassNode";
import InterfaceNode from "./Nodes/InterfaceNode/InterfaceNode";
import StructNode from "./Nodes/StructNode/StructNode";
import RecordNode from "./Nodes/RecordNode/RecordNode";
import EnumNode from "./Nodes/EnumNode/EnumNode";
import FloatingEdge from "./FloatingEdge";
import "./index.css";

const nodeTypes = {
  classNode: ClassNode,
  interfaceNode: InterfaceNode,
  structNode: StructNode,
  recordNode: RecordNode,
  enumNode: EnumNode,
};

const edgeTypes = {
  floating: FloatingEdge,
};

const minimapStyle = {
  height: 120,
};

const onInit = (reactFlowInstance) =>
  console.log("flow loaded:", reactFlowInstance);

const OverviewFlow = () => {
  const [nodes, setNodes, onNodesChange] = useNodesState(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);
  const onConnect = useCallback(console.log("connected"));

  return (
    <ReactFlow
      nodes={nodes}
      edges={edges}
      onNodesChange={onNodesChange}
      onEdgesChange={onEdgesChange}
      onConnect={onConnect}
      onInit={onInit}
      fitView
      attributionPosition="top-right"
      nodeTypes={nodeTypes}
      edgeTypes={edgeTypes}
    >
      <MiniMap style={minimapStyle} zoomable pannable />
      <Controls />
      <Background color="#aaa" gap={16} />
    </ReactFlow>
  );
};

export default OverviewFlow;
