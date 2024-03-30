import ELK from "elkjs/lib/elk.bundled.js";
import React, { useCallback, useEffect, useState } from "react";
import ReactFlow, {
  ReactFlowProvider,
  Panel,
  useNodesState,
  useEdgesState,
  useReactFlow,
  MiniMap,
  Controls,
} from "reactflow";
import "./overview.css";
import BlockNode from "../Nodes/Flowchart/BlockNode/BlockNode.js";
import TerminalNode from "../Nodes/Flowchart/TerminalNode/TerminalNode.js";
import ConditionNode from "../Nodes/Flowchart/ConditionNode/ConditionNode.js";
import CycleStartNode from "../Nodes/Flowchart/CycleStartNode/CycleStartNode.js";
import CycleEndNode from "../Nodes/Flowchart/CycleEndNode/CycleEndNode.js";
import "../Nodes/Flowchart/FlowchartNodes.css";

import "reactflow/dist/style.css";

import Markers from "../Markers/Markers.js";
import ArrowEdge from "../Edges/ArrowEdge.js";

const nodeTypes = {
  blockNode: BlockNode,
  terminalNode: TerminalNode,
  conditionNode: ConditionNode,
  openCycleNode: CycleStartNode,
  closeCycleNode: CycleEndNode,
};

const edgeTypes = {
  arrow: ArrowEdge,
};

const minZoom = 0.1;
const maxZoom = 1000;
const minimapStyle = {
  height: 120,
};

const elk = new ELK();
const elkOptions = {
  "elk.algorithm": "org.eclipse.elk.layered",
  "elk.direction": "DOWN",
  "org.eclipse.elk.layered.spacing.nodeNodeBetweenLayers": 80,
  "org.eclipse.elk.layered.spacing.edgeNodeBetweenLayers": 40,
  "org.eclipse.elk.direction": "DOWN",
  "org.eclipse.elk.layered.nodePlacement.strategy": "LINEAR_SEGMENTS",
};

const getLayoutedElements = async (nodes, edges, options = {}) => {
  const graph = {
    id: "root",
    layoutOptions: options,
    children: nodes,
    edges: edges,
  };

  try {
    const layoutedGraph = await elk.layout(graph);
    return {
      nodes: layoutedGraph.children.map((node) => ({
        ...node,
        position: { x: node.x, y: node.y },
      })),
      edges: layoutedGraph.edges,
    };
  } catch (error) {
    console.warn("Error occurred while laying out elements:", error);
    return { nodes: [], edges: [] };
  }
};

function LayoutFlow({ initialNodes, initialEdges }) {
  const { fitView } = useReactFlow();

  const [nodes, setNodes, onNodesChange] = useNodesState([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);

  const onLayout = useCallback(() => {
    getLayoutedElements(nodes, edges, elkOptions).then(
      ({ nodes: layoutedNodes, edges: layoutedEdges }) => {
        setNodes(layoutedNodes);
        setEdges(layoutedEdges);
      }
    );
  }, [nodes, edges]);

  useEffect(() => {
    var nodesWithPosition = initialNodes.map((node) => ({
      ...node,
      position: { x: 0, y: 0 },
    }));
    setNodes(nodesWithPosition);
    setEdges(initialEdges);
  }, [initialNodes, initialEdges]);

  const onConnect = useCallback((params) => console.log("onConnect"), []);

  return (
    <>
      <Markers></Markers>
      <ReactFlow
        minZoom={minZoom}
        maxZoom={maxZoom}
        fitView
        nodes={nodes}
        edges={edges}
        onConnect={onConnect}
        onNodesChange={onNodesChange}
        onEdgesChange={onEdgesChange}
        nodeTypes={nodeTypes}
        edgeTypes={edgeTypes}
      >
        <Panel position="top-right">
          <button onClick={() => onLayout()}>Use layout</button>
        </Panel>
        <MiniMap style={minimapStyle} zoomable pannable />
        <Controls />
      </ReactFlow>
    </>
  );
}

const FlowchartLayout = ({ initialNodes, initialEdges }) => {
  return (
    <ReactFlowProvider>
      <div style={{ height: "100vh" }}>
        <LayoutFlow initialEdges={initialEdges} initialNodes={initialNodes} />
      </div>
    </ReactFlowProvider>
  );
};

export default FlowchartLayout;
