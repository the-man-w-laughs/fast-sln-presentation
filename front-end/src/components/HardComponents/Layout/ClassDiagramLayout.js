import ELK from "elkjs/lib/elk.bundled.js"; // Подключение библиотеки ELK для автоматического размещения элементов на диаграмме
import React, { useCallback, useEffect, useState } from "react"; // Подключение React и необходимых хуков
import ReactFlow, { // Подключение ReactFlow и его компонентов
  ReactFlowProvider,
  Panel,
  useNodesState,
  useEdgesState,
  useReactFlow,
  MiniMap,
  Controls,
} from "reactflow";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"; // Подключение FontAwesome для использования иконок
import { faCompressArrowsAlt } from "@fortawesome/free-solid-svg-icons"; // Иконка для кнопки сжатия диаграммы

import "reactflow/dist/style.css"; // Стили для ReactFlow
import "../Nodes/ClassDiagram/ClassDiagramNodes.css"; // Стили для узлов диаграммы классов
import "./overview.css"; // Стили для обзора

// Подключение компонентов для узлов диаграммы классов
import ClassNode from "../Nodes/ClassDiagram/ClassNode/ClassNode.js";
import EnumNode from "../Nodes/ClassDiagram/EnumNode/EnumNode.js";
import InterfaceNode from "../Nodes/ClassDiagram/InterfaceNode/InterfaceNode.js";
import StructNode from "../Nodes/ClassDiagram/StructNode/StructNode.js";
import RecordNode from "../Nodes/ClassDiagram/RecordNode/RecordNode.js";
import DelegateNode from "../Nodes/ClassDiagram/DelegateNode/DelegateNode.js";

// Подключение компонентов для рёбер диаграммы классов
import ImplementationEdge from "../Edges/ImplementationEdge.js";
import Markers from "../Markers/Markers.js"; // Компоненты для маркеров рёбер
import InheritanceEdge from "../Edges/InheritanceEdge.js";
import AggregationEdge from "../Edges/AggregationEdge.js";
import CompositonEdge from "../Edges/CompositonEdge.js";
import AssociationEdge from "../Edges/AssociationEdge.js";

const minZoom = 0.1; // Минимальное увеличение
const maxZoom = 1000; // Максимальное увеличение

const elk = new ELK(); // Создание экземпляра ELK для автоматического размещения элементов на диаграмме

const elkOptions = {
  "elk.algorithm": "org.eclipse.elk.force", // Алгоритм размещения элементов
  "org.eclipse.elk.disco.componentCompaction.componentLayoutAlgorithm": "box", // Алгоритм компактной компоновки
  // "elk.direction": "DOWN", // Направление размещения
};

// Функция для получения размещенных элементов на диаграмме
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

// Типы узлов на диаграмме
const nodeTypes = {
  classNode: ClassNode,
  interfaceNode: InterfaceNode,
  structNode: StructNode,
  recordNode: RecordNode,
  enumNode: EnumNode,
  delegateNode: DelegateNode,
};

// Типы рёбер на диаграмме
const edgeTypes = {
  implementation: ImplementationEdge,
  inheritance: InheritanceEdge,
  aggregation: AggregationEdge,
  composition: CompositonEdge,
  association: AssociationEdge,
};

// Стили для мини-карты
const minimapStyle = {
  height: 120,
};

// Компонент для размещения элементов на диаграмме
function LayoutFlow({ initialNodes, initialEdges }) {
  const { fitView } = useReactFlow();

  const [nodes, setNodes, onNodesChange] = useNodesState([]);
  const [edges, setEdges, onEdgesChange] = useEdgesState([]);

  const onConnect = useCallback((params) => console.log("onConnect"), []);

  // Функция для размещения элементов на диаграмме
  const onLayout = useCallback(() => {
    getLayoutedElements(nodes, edges, elkOptions).then(
      ({ nodes: layoutedNodes, edges: layoutedEdges }) => {
        setNodes(layoutedNodes);
        setEdges(layoutedEdges);
      }
    );
  }, [nodes, edges]);

  // Инициализация узлов и рёбер при загрузке
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
        <Panel
          className="position-absolute bottom-0 start-0"
          style={{ paddingLeft: "2rem" }}
        >
          <button className="btn btn-primary" onClick={() => onLayout()}>
            <FontAwesomeIcon icon={faCompressArrowsAlt} className="me-2" />
            Группировать
          </button>
        </Panel>
        <MiniMap style={minimapStyle} zoomable pannable />
        <Controls />
      </ReactFlow>
    </>
  );
}

// Компонент для отображения диаграммы классов
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
