import ELK from "elkjs/lib/elk.bundled.js";
import React, {
  useCallback,
  useEffect,
  useState,
  useImperativeHandle,
} from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCompressArrowsAlt } from "@fortawesome/free-solid-svg-icons";
import ReactFlow, {
  ReactFlowProvider,
  Panel,
  useNodesState,
  useEdgesState,
  useReactFlow,
  MiniMap,
  Controls,
} from "reactflow";
import BlockNode from "../Nodes/Flowchart/BlockNode/BlockNode.js";
import TerminalNode from "../Nodes/Flowchart/TerminalNode/TerminalNode.js";
import ConditionNode from "../Nodes/Flowchart/ConditionNode/ConditionNode.js";
import CycleStartNode from "../Nodes/Flowchart/CycleStartNode/CycleStartNode.js";
import CycleEndNode from "../Nodes/Flowchart/CycleEndNode/CycleEndNode.js";
import "../Nodes/Flowchart/FlowchartNodes.css";

import "reactflow/dist/style.css";
import "./overview.css";

import Markers from "../Markers/Markers.js";
import ArrowEdge from "../Edges/ArrowEdge.js";
import DownloadButton from "../DownloadButton.js";

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
  "elk.layered.spacing.nodeNodeBetweenLayers": "30", // Increase spacing between layers
  "elk.spacing.nodeNode": "30", // Increase spacing between nodes
  "elk.layered.layering.strategy": "LONGEST_PATH", // Use the longest path layering strategy
  "elk.direction": "DOWN", // Layout direction
  "elk.edgeRouting": "POLYLINE",
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

function LayoutFlow({ initialNodes, initialEdges, ref, refresh }) {
  const { fitView } = useReactFlow();

  const [updateGraph, setupdateGraph] = useState(0);
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

  const autoLayout = useCallback(() => {
    onLayout();
    setTimeout(() => fitView(), 500);
  }, [onLayout, fitView]);

  useEffect(() => {
    //onLayout();
  }, [refresh]);

  useEffect(() => {
    var nodesWithPosition = initialNodes.map((node) => ({
      ...node,
      position: { x: 0, y: 0 },
    }));

    setNodes(nodesWithPosition);
    setEdges(initialEdges);

    setTimeout(() => setupdateGraph((value) => value + 1), 250);
  }, [initialNodes, initialEdges]);

  useEffect(() => {
    setTimeout(() => autoLayout(), 250);
  }, [updateGraph]);

  const onConnect = useCallback((params) => console.log("onConnect"), []);

  return (
    <>
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
        <Markers></Markers>
        <Panel
          className="position-absolute bottom-0 start-0"
          style={{ paddingLeft: "2rem" }}
        >
          <button className="btn btn-primary me-2" onClick={() => autoLayout()}>
            <FontAwesomeIcon icon={faCompressArrowsAlt} className="me-2" />
            Группировать
          </button>
          <DownloadButton />
        </Panel>
        <MiniMap style={minimapStyle} zoomable pannable />
        <Controls />
      </ReactFlow>
    </>
  );
}

const FlowchartLayout = ({ initialNodes, initialEdges, childRef, refresh }) => {
  return (
    <ReactFlowProvider>
      <div className="flowchart-layout">
        <LayoutFlow
          initialEdges={initialEdges}
          initialNodes={initialNodes}
          refresh={refresh}
          ref={childRef}
        />
      </div>
    </ReactFlowProvider>
  );
};

export default FlowchartLayout;
