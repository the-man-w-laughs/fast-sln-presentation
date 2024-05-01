import React, { useState } from "react";
import ClassDiagramLayout from "../../components/Layout/ClassDiagramLayout";
import "bootstrap/dist/css/bootstrap.min.css"; // Import Bootstrap CSS
import "./ClassDiagramPage.css"; // Import CSS file

const ClassDiagramPage = () => {
  const [inputType, setInputType] = useState("pat_author_repo");
  const [pat, setPat] = useState("");
  const [author, setAuthor] = useState("");
  const [repoName, setRepoName] = useState("");
  const [file, setFile] = useState(null);
  const [initialNodes, setInitialNodes] = useState([]);
  const [initialEdges, setInitialEdges] = useState([]);
  const [loading, setLoading] = useState(false);

  const handleInputChange = (event) => {
    setInputType(event.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    let requestBody;
    let url;
    let headers;
    if (inputType === "pat_author_repo") {
      requestBody = JSON.stringify({
        pat: pat,
        owner: author,
        repoName: repoName,
      });
      url = "http://localhost:5137/class-diagram/github";
      headers = {
        "Content-Type": "application/json",
      };
    } else if (inputType === "file") {
      requestBody = new FormData();
      requestBody.append("file", file);
      url = "http://localhost:5137/class-diagram/zip-file";
    }

    try {
      const response = await fetch(url, {
        method: "POST",
        body: requestBody,
        headers: headers,
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
          className="form-content col-md-2 bg-light d-flex flex-column justify-content-center"
          onSubmit={handleSubmit}
        >
          <div className="form-group">
            <label htmlFor="inputTypeSelect">Выберите способ ввода:</label>
            <select
              className="form-control"
              id="inputTypeSelect"
              value={inputType}
              onChange={handleInputChange}
            >
              <option value="pat_author_repo">Github репозиторий</option>
              <option value="file">Загрузить архив</option>
            </select>
          </div>
          {inputType === "pat_author_repo" && (
            <>
              <div className="form-group">
                <label htmlFor="patInput">
                  PAT (Персональный ключ доступа):
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="patInput"
                  value={pat}
                  onChange={(e) => setPat(e.target.value)}
                />
              </div>
              <div className="form-group">
                <label htmlFor="authorInput">Автор:</label>
                <input
                  type="text"
                  className="form-control"
                  id="authorInput"
                  value={author}
                  onChange={(e) => setAuthor(e.target.value)}
                />
              </div>
              <div className="form-group">
                <label htmlFor="repoNameInput">Название репозитория:</label>
                <input
                  type="text"
                  className="form-control"
                  id="repoNameInput"
                  value={repoName}
                  onChange={(e) => setRepoName(e.target.value)}
                />
              </div>
            </>
          )}
          {inputType === "file" && (
            <div className="form-group">
              <label htmlFor="codeFileInput">Загрузить архив:</label>
              <input
                type="file"
                className="form-control-file"
                id="codeFileInput"
                onChange={(e) => setFile(e.target.files[0])}
              />
            </div>
          )}
          <button
            type="submit"
            className="btn btn-primary mt-3 align-self-stretch mx-5"
            disabled={loading}
          >
            {loading ? "Загрузка..." : "Подтвердить"}
          </button>
        </form>
        <div className="col-md-10">
          <ClassDiagramLayout
            initialNodes={initialNodes}
            initialEdges={initialEdges}
          />
        </div>
      </div>
    </div>
  );
};

export default ClassDiagramPage;
