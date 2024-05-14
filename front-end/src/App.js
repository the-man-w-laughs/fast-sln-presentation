import logo from "./logo.svg";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import FlowchartPage from "./Pages/FlowchartPage/FlowchartPage";
import DefaultPage from "./Pages/DefultPage/DefaultPage";
import ClassDiagramPage from "./Pages/ClassDiagramPage/ClassDiagramPage";
import HomePage from "./Pages/HomePage/HomePage";
import LoginPage from "./Pages/LoginPage/LoginPage";
import AdminPage from "./Pages/AdminPage/AdminPage";
import { Navbar, Nav } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUser,
  faSignOutAlt,
  faSignInAlt,
  faRocket,
  faSitemap,
  faProjectDiagram,
  faUserTie,
  faShieldAlt,
  faUsersCog,
  faTasks,
  faWrench,
} from "@fortawesome/free-solid-svg-icons";

import { Roles } from "./Utils/Roles";
import { getAccessToken, getUserInfo } from "./Utils/LocalStorage";
import {
  getUserDataByToken,
  makeAuthenticatedRequest,
} from "./Utils/ApiService";
import { useEffect, useState } from "react";
import "./App.css";
import ProfilePage from "./Pages/ProfilePage/ProfilePage";

function App() {
  const [userInfo, setUserInfo] = useState(null);

  const handleLogout = () => {
    setUserInfo(null);
    localStorage.clear();
  };

  // Используем useEffect для загрузки данных пользователя по токену
  useEffect(() => {
    async function fetchUserData() {
      try {
        const data = await makeAuthenticatedRequest(getUserDataByToken);
        setUserInfo(data);
      } catch {
        handleLogout();
      }
    }
    fetchUserData();
  }, []);

  return (
    <body
      style={{ minHeight: "100vh", display: "flex", flexDirection: "column" }}
    >
      <Router>
        <Navbar
          bg="light"
          expand="lg"
          style={{ padding: "0px" }}
          className="custom-navbar"
        >
          <Navbar.Brand href="/home" className="custom-brand header">
            <img
              src="/CSharpGraph.svg"
              alt="Rocket Icon"
              style={{ height: "60px" }} // Adjust size as needed
            />
          </Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="mr-auto">
              {userInfo && (
                <>
                  <Nav.Link href="/class-diagram-page">
                    <FontAwesomeIcon icon={faProjectDiagram} className="me-2" />
                    Диаграмма классов
                  </Nav.Link>
                  <Nav.Link href="/flowchart-page">
                    <FontAwesomeIcon icon={faSitemap} className="me-2" />
                    Блок-схема
                  </Nav.Link>
                </>
              )}
            </Nav>
            <Nav className="ms-auto">
              {userInfo && userInfo.roleId === Roles.ADMINISTRATOR && (
                <Nav.Link href="/admin" className="margin-right-5">
                  <FontAwesomeIcon icon={faWrench} className="me-2" />
                  Администратор
                </Nav.Link>
              )}
              {userInfo && (
                <Nav.Link href="/profile" className="margin-right-5">
                  <FontAwesomeIcon icon={faUser} className="me-2" />
                  {userInfo.name}
                </Nav.Link>
              )}
              {userInfo == null && (
                <Nav.Link href="/login" className="margin-right-5">
                  <FontAwesomeIcon icon={faSignInAlt} className="me-2" />
                  Войти
                </Nav.Link>
              )}
            </Nav>
          </Navbar.Collapse>
        </Navbar>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/home" element={<HomePage />} />
          <Route path="/class-diagram-page" element={<ClassDiagramPage />} />
          <Route path="/flowchart-page" element={<FlowchartPage />} />
          <Route
            path="/admin"
            element={<AdminPage handleLogout={handleLogout} />}
          />
          <Route
            path="/login"
            element={<LoginPage setUserInfo={setUserInfo} />}
          />
          <Route
            path="/profile"
            element={<ProfilePage handleLogout={handleLogout} />}
          />
          <Route
            path="/profile/:id"
            element={<ProfilePage handleLogout={handleLogout} />}
          />
          <Route path="*" element={<DefaultPage />} />
        </Routes>
        <footer
          style={{
            backgroundColor: "#121F3B",
            color: "white",
            padding: "1rem",
            marginTop: "auto",
          }}
        >
          <p style={{ textAlign: "center" }}>© 2024 CSharpGraph</p>
        </footer>
      </Router>
    </body>
  );
}

export default App;
