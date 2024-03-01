import logo from "./logo.svg";
import "./App.css";
import OverviewFlow from "./components/OverviewFlow";
// import LayoutExample from "./components/Layout/DagreLayoutExample";
// import DagreLayoutExample from "./components/DagreLayoutExample";
import ElkLayoutExample from "./components/Layout/ElkLayoutExample";

function App() {
  return (
    <div style={{ height: "100vh" }}>
      <OverviewFlow></OverviewFlow>

      {/* <LayoutExample></LayoutExample> */}
      {/* <DagreLayoutExample></DagreLayoutExample> */}
      {/* <ElkLayoutExample></ElkLayoutExample> */}
      {/* <NodeAsHandleFlow></NodeAsHandleFlow> */}
    </div>
  );
}

export default App;
