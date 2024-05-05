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
    <Router>
      <Navbar bg="light" expand="lg">
        <Navbar.Brand href="/home" className="custom-brand header">
          <FontAwesomeIcon icon={faRocket} className="me-2" />
          fast-sln-presentation
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
            {userInfo && userInfo.RoleId === Roles.ADMIN && (
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
        <Route path="/admin" element={<AdminPage />} />
        <Route
          path="/login"
          element={<LoginPage setUserInfo={setUserInfo} />}
        />
        <Route
          path="/profile"
          element={<ProfilePage handleLogout={handleLogout} />}
        />
        <Route path="*" element={<DefaultPage />} />
      </Routes>
    </Router>
  );
}

export default App;
