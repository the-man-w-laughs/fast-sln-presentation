import React, { useState } from "react";
import FlowchartLayout from "../../components/Layout/FlowchartLayout";
import "bootstrap/dist/css/bootstrap.min.css";
import "./FlowchartPage.css";

const FlowchartPage = () => {
  const [code, setCode] = useState("");
  const [initialNodes, setInitialNodes] = useState([]);
  const [initialEdges, setInitialEdges] = useState([]);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const response = await fetch("http://localhost:5137/flowchart", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(code),
      });

      if (response.ok) {
        const { initialNodes, initialEdges } = await response.json();
        setInitialNodes(initialNodes);
        setInitialEdges(initialEdges);
        console.log("Successfully updated initial nodes and edges");
      } else {
        console.log("Error occurred while fetching data");
      }
    } catch (error) {
      console.error("Error submitting code:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container-fluid">
      <div className="row">
        <form
          className="form-content col-md-4 bg-light d-flex flex-column justify-content-center"
          onSubmit={handleSubmit}
        >
          <label htmlFor="codeTextArea">Paste code</label>
          <textarea
            className="form-control"
            id="codeTextArea"
            value={code}
            onChange={(e) => setCode(e.target.value)}
          ></textarea>
          <button
            type="submit"
            className="btn btn-primary mt-3 align-self-stretch mx-5"
            disabled={loading}
          >
            {loading ? "Loading..." : "Submit"}
          </button>
        </form>
        <div className="col-md-8">
          <FlowchartLayout
            initialNodes={initialNodes}
            initialEdges={initialEdges}
          />
        </div>
      </div>
    </div>
  );
};

export default FlowchartPage;
