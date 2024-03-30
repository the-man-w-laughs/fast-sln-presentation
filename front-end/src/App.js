import logo from "./logo.svg";
import "./App.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import FlowchartPage from "./Pages/FlowchartPage/FlowchartPage";
import DefaultPage from "./Pages/DefultPage/DefaultPage";
import ClassDiagramPage from "./Pages/ClassDiagramPage/ClassDiagramPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/class-diagram-page" element={<ClassDiagramPage />} />
        <Route path="/flowchart-page" element={<FlowchartPage />} />
        <Route path="*" element={<DefaultPage />} />
      </Routes>
    </Router>
  );
}

export default App;
