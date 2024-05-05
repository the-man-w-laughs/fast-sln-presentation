import React, { useState } from "react";
import { Container, Row, Col, ListGroup } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUsers,
  faClipboardList,
  faRubleSign,
} from "@fortawesome/free-solid-svg-icons";
import UsersPage from "./UsersPage/UsersPage";
import PlansPage from "./PlansPage/PlansPage";
import { makeAuthenticatedRequest } from "../../Utils/ApiService";
import "./AdminPage.css";

function AdminPage({ handleLogout }) {
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
          {selectedPage === "users" && (
            <UsersPage handleLogout={handleLogout} />
          )}
          {selectedPage === "plans" && (
            <PlansPage handleLogout={handleLogout} />
          )}
        </Col>
      </Row>
    </Container>
  );
}

export default AdminPage;
