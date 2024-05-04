import React from "react";
import { Navbar, Nav, Container, Row, Col, ListGroup } from "react-bootstrap";

function AdminPage() {
  return (
    <div>
      <Container fluid>
        <Row>
          {/* Боковая панель */}
          <Col md={2} className="p-3">
            <ListGroup>
              <ListGroup.Item action href="admin">
                Пользователи
              </ListGroup.Item>
              <ListGroup.Item action href="admin">
                Планы
              </ListGroup.Item>
            </ListGroup>
          </Col>
        </Row>
      </Container>
    </div>
  );
}

export default AdminPage;
