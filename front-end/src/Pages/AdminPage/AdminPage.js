import React, { useState } from "react";
import { Container, Row, Col, ListGroup } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUsers,
  faClipboardList,
  faRubleSign,
} from "@fortawesome/free-solid-svg-icons";
import { makeAuthenticatedRequest } from "../../Utils/ApiService";
import "./AdminPage.css";

function Users() {
  return <div>Здесь будут пользователи</div>;
}

function Plans() {
  return <div>Здесь будут планы</div>;
}

function AdminPage() {
  // Создаем состояние для отслеживания выбранной подстраницы
  const [selectedPage, setSelectedPage] = useState("users");

  return (
    <Container fluid>
      <Row>
        {/* Боковая панель */}
        <Col md={2} className="p-3">
          <ListGroup>
            <ListGroup.Item
              action
              active={selectedPage === "users"}
              onClick={() => setSelectedPage("users")}
            >
              <FontAwesomeIcon icon={faUsers} /> {/* Иконка пользователей */}
              <span className="ml-2">Пользователи</span>
            </ListGroup.Item>
            <ListGroup.Item
              action
              active={selectedPage === "plans"}
              onClick={() => setSelectedPage("plans")}
            >
              <FontAwesomeIcon icon={faRubleSign} /> {/* Иконка планов */}
              <span className="ml-2">Планы</span>
            </ListGroup.Item>
          </ListGroup>
        </Col>

        {/* Основное содержимое */}
        <Col md={10} className="p-3">
          {selectedPage === "users" && <Users />}
          {selectedPage === "plans" && <Plans />}
        </Col>
      </Row>
    </Container>
  );
}

export default AdminPage;
