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

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faCompressArrowsAlt,
  faCheckSquare,
  faSquare,
  faCog,
} from "@fortawesome/free-solid-svg-icons";

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
import DownloadButton from "../DownloadButton.js";
import GroupingSettingsModal from "./GroupSettingsModal.js";

const minZoom = 0.01;
const maxZoom = 1000;

const elk = new ELK();

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
  const [showSettingsModal, setShowSettingsModal] = useState(false);
  const [elkOptions, setElkOptions] = useState({
    "elk.algorithm": "org.eclipse.elk.force",
    "elk.direction": "DOWN",
    "org.eclipse.elk.spacing.nodeNode": "100",
    "org.eclipse.elk.layered.spacing.edgeNodeBetweenLayers": "50",
    "org.eclipse.elk.layered.spacing.nodeNodeBetweenLayers": "50",
  });

  const handleApplySettings = (settings) => {
    setElkOptions((prevOptions) => ({
      ...prevOptions,
      ...settings,
    }));
  };

  const { fitView } = useReactFlow();

  const [updateGraph, setupdateGraph] = useState(0);
  const [nodes, setNodes, onNodesChange] = useNodesState([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);

  const [showContent, setShowContent] = useState(true);

  const toggleContentVisibility = () => {
    setShowContent((previousValue) => !previousValue);
    setNodes(
      nodes.map((node) => ({
        ...node,
        data: {
          ...node.data,
          showContent: !showContent,
        },
      }))
    );
  };

  const onConnect = useCallback((params) => console.log("onConnect"), []);

  const onLayout = useCallback(() => {
    getLayoutedElements(nodes, edges, elkOptions).then(
      ({ nodes: layoutedNodes, edges: layoutedEdges }) => {
        setNodes(layoutedNodes);
        setEdges(layoutedEdges);
      }
    );
  }, [nodes, edges, elkOptions]);

  useEffect(() => {
    var nodesWithPosition = initialNodes.map((node) => ({
      ...node,
      position: { x: 0, y: 0 },
      data: {
        ...node.data,
        showContent: showContent,
      },
    }));
    setNodes(nodesWithPosition);
    setEdges(initialEdges);
    setTimeout(() => setupdateGraph((value) => value + 1), 250);
  }, [initialNodes, initialEdges]);

  const autoLayout = useCallback(() => {
    onLayout();
    setTimeout(() => fitView(), 750);
  }, [onLayout, fitView]);

  useEffect(() => {
    setTimeout(() => autoLayout(), 250);
  }, [updateGraph]);

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
        <Panel
          className="position-absolute bottom-0 start-0"
          style={{ paddingLeft: "2rem" }}
        >
          <button className="btn btn-primary me-2" onClick={() => autoLayout()}>
            <FontAwesomeIcon icon={faCompressArrowsAlt} className="me-2" />
            Группировать
          </button>
          <button
            className="btn btn-primary me-2"
            onClick={() => setShowSettingsModal(true)}
          >
            <FontAwesomeIcon icon={faCog} className="me-2" />
            Настройка группировки
          </button>
          <GroupingSettingsModal
            show={showSettingsModal}
            onHide={() => {
              setShowSettingsModal(false);
            }}
            applySettings={handleApplySettings}
            defaultSettings={elkOptions}
          />
          <button
            className="btn btn-primary me-2"
            onClick={toggleContentVisibility}
          >
            <FontAwesomeIcon
              icon={showContent ? faCheckSquare : faSquare}
              className="me-2"
            />
            {"Показать содержимое"}
          </button>
          <DownloadButton></DownloadButton>
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
