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

import "reactflow/dist/style.css";
import "../Nodes/ClassDiagram/ClassDiagramNodes.css";
import "./overview.css";

import ClassNode from "../Nodes/ClassDiagram/ClassNode/ClassNode.js";
import EnumNode from "../Nodes/ClassDiagram/EnumNode/EnumNode.js";
import InterfaceNode from "../Nodes/ClassDiagram/InterfaceNode/InterfaceNode.js";
import StructNode from "../Nodes/ClassDiagram/StructNode/StructNode.js";
import RecordNode from "../Nodes/ClassDiagram/RecordNode/RecordNode.js";
import DelegateNode from "../Nodes/ClassDiagram/DelegateNode/DelegateNode.js";

import ImplementationEdge from "../Edges/ImplementationEdge.js";
import Markers from "../Markers/Markers.js";
import InheritanceEdge from "../Edges/InheritanceEdge.js";
import AggregationEdge from "../Edges/AggregationEdge.js";
import CompositonEdge from "../Edges/CompositonEdge.js";
import AssociationEdge from "../Edges/AssociationEdge.js";

const minZoom = 0.1;
const maxZoom = 1000;

const elk = new ELK();

const elkOptions = {
  "elk.algorithm": "org.eclipse.elk.force",
  "org.eclipse.elk.disco.componentCompaction.componentLayoutAlgorithm": "box",
  // "elk.direction": "DOWN",
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
  association: AssociationEdge,
};

const minimapStyle = {
  height: 120,
};

function LayoutFlow({ initialNodes, initialEdges }) {
  const { fitView } = useReactFlow();

  const [nodes, setNodes, onNodesChange] = useNodesState([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);

  const onConnect = useCallback((params) => console.log("onConnect"), []);

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
          <button onClick={() => onLayout()}>Группировать</button>
        </Panel>
        <MiniMap style={minimapStyle} zoomable pannable />
        <Controls />
      </ReactFlow>
    </>
  );
}

const ClassDiagramLayout = ({ initialNodes, initialEdges }) => {
  return (
    <ReactFlowProvider>
      <div className="flowchart-layout">
        <LayoutFlow initialEdges={initialEdges} initialNodes={initialNodes} />
      </div>
    </ReactFlowProvider>
  );
};

export default ClassDiagramLayout;
