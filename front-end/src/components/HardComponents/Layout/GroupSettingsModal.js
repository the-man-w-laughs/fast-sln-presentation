import React, { useState } from "react";
import { Modal, Card, Button, Form } from "react-bootstrap";

const GroupingSettingsModal = ({
  show,
  onHide,
  applySettings,
  defaultSettings,
}) => {
  const [selectedOption, setSelectedOption] = useState(
    defaultSettings["elk.algorithm"]
  );
  const [direction, setDirection] = useState(defaultSettings["elk.direction"]);

  const handleOptionChange = (option) => {
    setSelectedOption(option);
  };

  const handleDirectionChange = (e) => {
    setDirection(e.target.value);
  };

  const handleApplySettings = () => {
    applySettings({
      "elk.algorithm": selectedOption,
      "elk.direction": direction,
    });
    onHide();
  };

  return (
    <Modal
      show={show}
      onHide={handleApplySettings}
      className="modal-lg"
      centered
    >
      <Modal.Header closeButton>
        <Modal.Title className="w-100 text-center">
          Настройки группировки
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <div className="d-flex flex-row">
          <Card
            className="m-2"
            border={
              selectedOption === "org.eclipse.elk.force"
                ? "primary"
                : "secondary"
            }
            style={{ width: "22rem", borderWidth: "3px" }} // Увеличиваем ширину карточки
            onClick={() => handleOptionChange("org.eclipse.elk.force")}
          >
            <Card.Body className="d-flex flex-column align-items-center">
              <img
                src="/images/org-eclipse-elk-force_preview_force_layout.png"
                width={150}
                height={150}
                alt="Описание группировки 1."
                className="mb-3"
                style={{ maxWidth: "100%", maxHeight: "100%" }}
              />
              <Card.Title className="text-center">Силовая</Card.Title>
            </Card.Body>
          </Card>
          <Card
            className="m-2"
            border={
              selectedOption === "org.eclipse.elk.mrtree"
                ? "primary"
                : "secondary"
            }
            style={{ width: "22rem", borderWidth: "3px" }} // Увеличиваем ширину карточки
            onClick={() => handleOptionChange("org.eclipse.elk.mrtree")}
          >
            <Card.Body className="d-flex flex-column align-items-center">
              <img
                src="/images/org-eclipse-elk-mrtree_preview_mrtree_layout.png"
                width={150}
                height={150}
                alt="Описание группировки 1."
                className="mb-3"
                style={{ maxWidth: "100%", maxHeight: "100%" }}
              />
              <Card.Title className="text-center">Древовидная</Card.Title>
            </Card.Body>
          </Card>
          <Card
            className="m-2"
            border={
              selectedOption === "org.eclipse.elk.layered"
                ? "primary"
                : "secondary"
            }
            style={{ width: "22rem", borderWidth: "3px" }} // Увеличиваем ширину карточки
            onClick={() => handleOptionChange("org.eclipse.elk.layered")}
          >
            <Card.Body className="d-flex flex-column align-items-center">
              <img
                src="/images/org-eclipse-elk-layered_preview_layered_layout.png"
                width={150}
                height={150}
                alt="Описание группировки 1."
                className="mb-3"
                style={{ maxWidth: "100%", maxHeight: "100%" }}
              />
              <Card.Title className="text-center">Многослойная</Card.Title>
            </Card.Body>
          </Card>

          {/* <Card
            className="m-2"
            border={
              selectedOption === "org.eclipse.elk.radial"
                ? "primary"
                : "secondary"
            }
            style={{ width: "22rem", borderWidth: "3px" }}
            onClick={() => handleOptionChange("org.eclipse.elk.radial")}
          >
            <Card.Body className="d-flex flex-column align-items-center">
              <img
                src="/images/org-eclipse-elk-radial_preview_radial_layout.png"
                width={150}
                height={150}
                alt="Описание группировки 1."
                className="mb-3"
                style={{ maxWidth: "100%", maxHeight: "100%" }}
              />
              <Card.Title className="text-center">Радиальная</Card.Title>
            </Card.Body>
          </Card> */}
          {/* Добавьте другие карточки по аналогии */}
        </div>
        <Form.Group controlId="directionSelect">
          <Form.Label>Направление группировки:</Form.Label>
          <Form.Control
            as="select"
            value={direction}
            onChange={handleDirectionChange}
          >
            <option value="UP">Вверх</option>
            <option value="DOWN">Вниз</option>
            <option value="RIGHT">Вправо</option>
            <option value="LEFT">Влево</option>
            {/* Добавьте другие направления по аналогии */}
          </Form.Control>
        </Form.Group>
      </Modal.Body>
      <Modal.Footer className="d-flex justify-content-center">
        <Button
          variant="primary"
          className="me-2"
          onClick={handleApplySettings}
          style={{ minWidth: "120px" }}
        >
          ОК
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default GroupingSettingsModal;
