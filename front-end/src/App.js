import logo from "./logo.svg";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import FlowchartPage from "./Pages/FlowchartPage/FlowchartPage";
import DefaultPage from "./Pages/DefultPage/DefaultPage";
import ClassDiagramPage from "./Pages/ClassDiagramPage/ClassDiagramPage";
import HomePage from "./Pages/HomePage/HomePage";
import { Navbar, Nav } from "react-bootstrap";
import "./App.css";

function App() {
  return (
    <Router>
      <Navbar bg="light" expand="lg">
        <Navbar.Brand href="/" className="custom-brand">
          fast-sln-presentation
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="mr-auto">
            <Nav.Link href="/home">Домашняя</Nav.Link>
            <Nav.Link href="/class-diagram-page">Диаграмма классов</Nav.Link>
            <Nav.Link href="/flowchart-page">Блок-схема</Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Navbar>
      <Routes>
        <Route path="/home" element={<HomePage />} />
        <Route path="/class-diagram-page" element={<ClassDiagramPage />} />
        <Route path="/flowchart-page" element={<FlowchartPage />} />
        <Route path="*" element={<DefaultPage />} />
      </Routes>
    </Router>
  );
}

export default App;
