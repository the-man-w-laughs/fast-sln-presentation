import { initialNodes, initialEdges } from "./nodes-edges.js";
import ELK from "elkjs/lib/elk.bundled.js";
import React, { useCallback, useLayoutEffect } from "react";
import ReactFlow, {
  ReactFlowProvider,
  addEdge,
  Panel,
  useNodesState,
  useEdgesState,
  useReactFlow,
} from "reactflow";
import "./overview.css";

import "reactflow/dist/style.css";

const elk = new ELK();

const elkOptions = {
  "elk.algorithm": "org.eclipse.elk.disco",
  "org.eclipse.elk.disco.componentCompaction.componentLayoutAlgorithm":
    "radial",
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

function LayoutFlow() {
  const [nodes, setNodes, onNodesChange] = useNodesState(initialNodes);
  const [edges, setEdges, onEdgesChange] = useEdgesState(initialEdges);
  const { fitView } = useReactFlow();

  const onConnect = useCallback((params) => console.log("loaded"), []);
  const onLayout = useCallback(() => {
    const opts = elkOptions;
    const ns = nodes;
    const es = edges;
    getLayoutedElements(ns, es, opts).then(
      ({ nodes: layoutedNodes, edges: layoutedEdges }) => {
        setNodes(layoutedNodes);
        setEdges(layoutedEdges);
        window.requestAnimationFrame(() => {
          console.log(nodes);
          fitView();
        });
      }
    );
  }, [nodes, edges]);
  return (
    <ReactFlow
      nodes={nodes}
      edges={edges}
      onConnect={onConnect}
      onNodesChange={onNodesChange}
      onEdgesChange={onEdgesChange}
      fitView
    >
      <Panel position="top-right">
        <button onClick={() => onLayout()}>Use layout</button>
      </Panel>
    </ReactFlow>
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
