import {
  BrowserRouter as Router,
  Route,
  Routes,
  useLocation,
} from "react-router-dom";
import FlowchartPage from "./Pages/FlowchartPage/FlowchartPage";
import DefaultPage from "./Pages/DefaultPage/DefaultPage";
import ClassDiagramPage from "./Pages/ClassDiagramPage/ClassDiagramPage";
import HomePage from "./Pages/HomePage/HomePage";
import LoginPage from "./Pages/LoginPage/LoginPage";
import AdminPage from "./Pages/AdminPage/AdminPage";
import ProfilePage from "./Pages/ProfilePage/ProfilePage";
import { Navbar, Nav } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUser,
  faSignInAlt,
  faProjectDiagram,
  faSitemap,
  faWrench,
} from "@fortawesome/free-solid-svg-icons";

import { Roles } from "./Utils/Roles";
import {
  getUserDataByToken,
  makeAuthenticatedRequest,
} from "./Utils/ApiService";
import { useEffect, useState } from "react";
import "./App.css";

function App() {
  const [userInfo, setUserInfo] = useState(null);

  const handleLogout = () => {
    setUserInfo(null);
    localStorage.clear();
  };

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

  const hideFooterRoutes = ["/class-diagram-page", "/flowchart-page"];

  const Layout = ({ children }) => {
    const location = useLocation();

    return (
      <div
        style={{ minHeight: "100vh", display: "flex", flexDirection: "column" }}
      >
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
              style={{ height: "60px" }}
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
        <div style={{ flex: 1 }}>{children}</div>
        {!hideFooterRoutes.includes(location.pathname) && (
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
        )}
      </div>
    );
  };

  return (
    <Router>
      <Layout>
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
      </Layout>
    </Router>
  );
}

export default App;
