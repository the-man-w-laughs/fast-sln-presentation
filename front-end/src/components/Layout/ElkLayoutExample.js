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
import ClassNode from "../Nodes/ClassNode/ClassNode.js";
import EnumNode from "../Nodes/EnumNode/EnumNode.js";
import InterfaceNode from "../Nodes/InterfaceNode/InterfaceNode.js";
import StructNode from "../Nodes/StructNode/StructNode.js";
import RecordNode from "../Nodes/RecordNode/RecordNode.js";
import DelegateNode from "../Nodes/DelegateNode/DelegateNode.js";
import ImplementationEdge from "../Edges/ImplementationEdge.js";
import "../Nodes/Nodes.css";

import "reactflow/dist/style.css";
import { initialNodes, initialEdges } from "../initial-elements.js";
import Markers from "../Markers/Markers.js";
import InheritanceEdge from "../Edges/InheritanceEdge.js";
import FloatingEdge from "../Edges/FloatingEdge.js";
import AggregationEdge from "../Edges/AggregationEdge.js";
import CompositonEdge from "../Edges/CompositonEdge.js";

const minZoom = 0.1;
const maxZoom = 1000;

const initialNodesWithPosition = initialNodes.map((node) => ({
  ...node,
  position: { x: 0, y: 0 },
}));

const elk = new ELK();

const elkOptions = {
  "elk.algorithm": "org.eclipse.elk.disco",
  "org.eclipse.elk.disco.componentCompaction.componentLayoutAlgorithm":
    "radial",
  "elk.direction": "DOWN",
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
  classNode: ClassNode,
  interfaceNode: InterfaceNode,
  structNode: StructNode,
  recordNode: RecordNode,
  enumNode: EnumNode,
  delegateNode: DelegateNode,
};

const edgeTypes = {
  implementation: ImplementationEdge,
  inheritance: InheritanceEdge,
  aggregation: AggregationEdge,
  composition: CompositonEdge,
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

const ElkLayoutExample = () => {
  return (
    <ReactFlowProvider>
      <LayoutFlow />
    </ReactFlowProvider>
  );
};

export default ElkLayoutExample;
