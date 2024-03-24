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
import "../Nodes/Flowchart/FlowchartNodes.css";

import "reactflow/dist/style.css";
import { initialNodes, initialEdges } from "../initial-elements.js";
import Markers from "../Markers/Markers.js";
import ArrowEdge from "../Edges/ArrowEdge.js";

const minZoom = 0.1;
const maxZoom = 1000;

const initialNodesWithPosition = initialNodes.map((node) => ({
  ...node,
  position: { x: 0, y: 0 },
}));

const elk = new ELK();

const elkOptions = {
  "elk.algorithm": "org.eclipse.elk.layered",
  "elk.direction": "DOWN",
  "org.eclipse.elk.layered.spacing.nodeNodeBetweenLayers": 80,
  "org.eclipse.elk.layered.spacing.edgeNodeBetweenLayers": 40,
  "org.eclipse.elk.layered.nodePlacement.strategy": "SIMPLE",
};

const getLayoutedElements = (nodes, edges, options = {}) => {
  const graph = {
    id: "root",
    layoutOptions: options,
    children: nodes,
    edges: edges,
  };

  return elk
    .layout(graph)
    .then((layoutedGraph) => ({
      nodes: layoutedGraph.children.map((node) => ({
        ...node,
        position: { x: node.x, y: node.y },
      })),

      edges: layoutedGraph.edges,
    }))
    .catch(console.error);
};

const nodeTypes = {
  blockNode: BlockNode,
  terminalNode: TerminalNode,
  conditionNode: ConditionNode,
  cycleStartNode: CycleStartNode,
};

const edgeTypes = {
  arrow: ArrowEdge,
};

const minimapStyle = {
  height: 120,
};

function LayoutFlow() {
  const [nodes, setNodes, onNodesChange] = useNodesState(
    initialNodesWithPosition
  );
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);
  const { fitView } = useReactFlow();

  const onConnect = useCallback((params) => console.log("onConnect"), []);
  const onLayout = useCallback(() => {
    const opts = elkOptions;
    const ns = nodes;
    const es = edges;

    getLayoutedElements(ns, es, opts).then(
      ({ nodes: layoutedNodes, edges: layoutedEdges }) => {
        setNodes(layoutedNodes);
        setEdges(layoutedEdges);
        window.requestAnimationFrame(() => {
          setTimeout(fitView);
        });
      }
    );
  }, [nodes, edges]);

  const [reactFlowInstance, setReactFlowInstance] = useState(null);

  useEffect(() => {
    if (reactFlowInstance) {
      onLayout();
    }
  }, [reactFlowInstance]);

  const onInit = (rf) => {
    setReactFlowInstance(rf);
  };

  return (
    <>
      <Markers></Markers>
      <ReactFlow
        minZoom={minZoom}
        maxZoom={maxZoom}
        fitView
        onInit={onInit}
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

const FlowchartLayout = () => {
  return (
    <ReactFlowProvider>
      <LayoutFlow />
    </ReactFlowProvider>
  );
};

export default FlowchartLayout;