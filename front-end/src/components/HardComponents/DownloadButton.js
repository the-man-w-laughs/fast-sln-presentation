import React, { useState, useEffect } from "react";
import { Button, Modal, Form, Row, Col } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDownload, faTimes } from "@fortawesome/free-solid-svg-icons";
import {
  Panel,
  useReactFlow,
  getRectOfNodes,
  getTransformForBounds,
} from "reactflow";
import { toPng, toSvg, toJpeg } from "html-to-image";
import Markers from "./Markers/Markers";
import ReactDOM from "react-dom";

function downloadImage(dataUrl, fileName) {
  const a = document.createElement("a");
  a.setAttribute("download", fileName);
  a.setAttribute("href", dataUrl);
  a.click();
}

function DownloadButton() {
  const { getNodes } = useReactFlow();
  const [showModal, setShowModal] = useState(false);
  const [selectedFormat, setSelectedFormat] = useState("PNG");

  useEffect(() => {
    const targetElement = document.querySelector(".react-flow__viewport");
    if (targetElement) {
      const markersContainer = document.createElement("div");
      ReactDOM.render(<Markers />, markersContainer);
      targetElement.appendChild(markersContainer);
    }
  }, []);

  const handleExport = () => {
    const nodes = getNodes();
    const viewportElement = document.querySelector(".react-flow__viewport");
    const nodesBounds = getRectOfNodes(nodes);
    const imageWidth = nodesBounds.width * 1.25;
    const imageHeight = nodesBounds.height * 1.25;
    const transform = getTransformForBounds(
      nodesBounds,
      imageWidth,
      imageHeight,
      0.01,
      5
    );

    const transformOptions = {
      backgroundColor: "#FFFFFF",
      width: imageWidth,
      height: imageHeight,
      style: {
        width: imageWidth,
        height: imageHeight,
        transform: `translate(${transform[0]}px, ${transform[1]}px) scale(${transform[2]})`,
      },
    };

    let exportFunction;
    let fileExtension;
    switch (selectedFormat) {
      case "PNG":
        exportFunction = toPng;
        fileExtension = "png";
        break;
      case "SVG":
        exportFunction = toSvg;
        fileExtension = "svg";
        break;
      case "JPEG":
        exportFunction = toJpeg;
        fileExtension = "jpg";
        break;
      default:
        return;
    }

    exportFunction(viewportElement, transformOptions).then((dataUrl) => {
      downloadImage(dataUrl, `CSharpGraph.${fileExtension}`);
    });

    setShowModal(false);
  };

  return (
    <>
      <Button
        variant="primary"
        onClick={() => setShowModal(true)}
        className="btn btn-primary"
      >
        <FontAwesomeIcon icon={faDownload} className="me-2" />
        Экспортировать
      </Button>
      <Modal show={showModal} onHide={() => setShowModal(false)} centered>
        <Modal.Header closeButton>
          <Modal.Title className="w-100 text-center">
            Выберите формат для экспорта
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <div className="d-flex justify-content-center">
            <Form.Group>
              <Form.Check
                type="radio"
                label="PNG"
                value="PNG"
                checked={selectedFormat === "PNG"}
                onChange={(e) => setSelectedFormat(e.target.value)}
                id="png"
                defaultChecked
              />
              <Form.Check
                type="radio"
                label="SVG"
                value="SVG"
                checked={selectedFormat === "SVG"}
                onChange={(e) => setSelectedFormat(e.target.value)}
                id="svg"
              />
              <Form.Check
                type="radio"
                label="JPEG"
                value="JPEG"
                checked={selectedFormat === "JPEG"}
                onChange={(e) => setSelectedFormat(e.target.value)}
                id="jpeg"
              />
            </Form.Group>
          </div>
        </Modal.Body>
        <Modal.Footer className="d-flex justify-content-center">
          <Button variant="secondary" onClick={() => setShowModal(false)}>
            <FontAwesomeIcon icon={faTimes} className="me-2" />
            Отмена
          </Button>
          <Button variant="primary" onClick={handleExport}>
            <FontAwesomeIcon icon={faDownload} className="me-2" />
            Загрузить
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}

export default DownloadButton;
