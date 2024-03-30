import logo from "./logo.svg";
import "./App.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import FlowchartLayout from "./components/Layout/FlowchartLayout";
import ClassDiagramLayout from "./components/Layout/ClassDiagramLayout";
import FlowchartPage from "./Pages/FlowchartPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/class-diagram" element={<ClassDiagramLayout />} />
        <Route path="/flowchart" element={<FlowchartLayout />} />
        <Route path="/flowchart-page" element={<FlowchartPage />} />
      </Routes>
    </Router>
  );
}

export default App;
