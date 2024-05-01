import React, { useState, useEffect } from "react";
import FlowchartLayout from "../../components/Layout/FlowchartLayout";
import "bootstrap/dist/css/bootstrap.min.css";
import "./FlowchartPage.css";

const FlowchartPage = () => {
  const [code, setCode] = useState("");
  const [initialNodes, setInitialNodes] = useState([]);
  const [initialEdges, setInitialEdges] = useState([]);
  const [loading, setLoading] = useState(false);

  const handleTabKeyPress = (e) => {
    if (e.key === "Tab") {
      e.preventDefault();
      const { selectionStart, selectionEnd } = e.target;
      const newCode =
        code.substring(0, selectionStart) + "\t" + code.substring(selectionEnd);
      setCode(newCode);
    }
  };

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

  useEffect(() => {
    const handleKeyDown = (event) => {
      if (event.ctrlKey && event.key === "Enter") {
        const formattedCode = formatCode(code);
        setCode(formattedCode);
      }
    };

    document.addEventListener("keydown", handleKeyDown);

    return () => {
      document.removeEventListener("keydown", handleKeyDown);
    };
  }, [code]);

  function formatCode(code) {
    // Ensure { and } are on separate lines without adding extra spaces
    code = code
      .replace(/\s*{\s*|\s*}\s*/g, (match) => {
        return "\n" + match.trim() + "\n";
      })
      .replaceAll(";", ";\n")
      .replace(/\n\s*\n/g, "\n");

    let tab = "\t";
    let result = "";
    let indent = 0;

    code.split("\n").forEach((line) => {
      let trimmedLine = line.trim();

      // Handle comments
      if (trimmedLine.startsWith("//") || trimmedLine.startsWith("/*")) {
        result += tab.repeat(indent) + trimmedLine + "\n";
        return;
      }

      // Handle braces
      if (trimmedLine.endsWith("}")) {
        indent = Math.max(0, indent - 1);
      }

      result += tab.repeat(indent) + trimmedLine + "\n";

      if (trimmedLine.endsWith("{")) {
        indent++;
      }
    });

    return result.trim();
  }

  return (
    <div className="container-fluid">
      <div className="row">
        <form
          className="form-content col-md-4 bg-light d-flex flex-column justify-content-center"
          onSubmit={handleSubmit}
        >
          <label htmlFor="codeTextArea">Введите код:</label>
          <textarea
            className="form-control"
            id="codeTextArea"
            value={code}
            onChange={(e) => setCode(e.target.value)}
            onKeyDown={handleTabKeyPress}
          ></textarea>
          <button
            type="submit"
            className="btn btn-primary mt-3 align-self-stretch mx-5"
            disabled={loading}
          >
            {loading ? "Загрузка..." : "Подтвердить"}
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
