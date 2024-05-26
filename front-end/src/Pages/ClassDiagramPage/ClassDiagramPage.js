import React, { useState } from "react";
import ClassDiagramLayout from "../../components/HardComponents/Layout/ClassDiagramLayout";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Swal from "sweetalert2";
import {
  faFolder,
  faDownload,
  faUpload,
  faKey,
  faUser,
  faFolderOpen,
  faSpinner,
  faPlay,
  faLink,
  faLinkSlash,
  faExternalLink,
  faExternalLinkAlt,
} from "@fortawesome/free-solid-svg-icons";
import {
  generateClassDiagramByFile,
  generateClassDiagramByGithub,
  makeAuthenticatedRequest,
} from "../../Utils/ApiService";
import "bootstrap/dist/css/bootstrap.min.css"; // Import Bootstrap CSS
import "./ClassDiagramPage.css"; // Import CSS file

const ClassDiagramPage = () => {
  const [inputType, setInputType] = useState("github_url");
  const [pat, setPat] = useState("");
  const [repoUrl, setRepoUrl] = useState("");
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

    try {
      let response;

      // Вызываем соответствующую функцию на основе inputType
      if (inputType === "github_url") {
        const urlPattern = /https:\/\/github.com\/([^/]+)\/([^/]+)/;
        const match = repoUrl.match(urlPattern);

        if (!match) {
          throw new Error("Ссылка на Github репозиторий в неверном формате.");
        }

        const [, author, repoName] = match;

        response = await makeAuthenticatedRequest(
          generateClassDiagramByGithub,
          pat,
          author,
          repoName
        );
      } else if (inputType === "file") {
        response = await makeAuthenticatedRequest(
          generateClassDiagramByFile,
          file
        );
      } else {
        throw new Error("Invalid inputType provided.");
      }

      const { initialNodes, initialEdges } = response;
      setInitialNodes(initialNodes);
      setInitialEdges(initialEdges);
      console.log("Успешно обновлены начальные узлы и ребра");
    } catch (error) {
      console.error("Ошибка отправки кода:", error);
      Swal.fire({
        icon: "error",
        title: "Ошибка",
        text: "Ошибка при генерации: " + error.message,
      });
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
            <label htmlFor="inputTypeSelect">
              <FontAwesomeIcon icon={faFolder} className="me-2" />
              Выберите способ ввода:
            </label>
            <select
              className="form-control"
              id="inputTypeSelect"
              value={inputType}
              onChange={handleInputChange}
            >
              <option value="github_url">Ссылка на GitHub репозиторий</option>
              <option value="file">Загрузить архив</option>
            </select>
          </div>
          {inputType === "github_url" && (
            <>
              <div className="form-group">
                <label htmlFor="patInput">
                  <FontAwesomeIcon icon={faKey} className="me-2" />
                  PAT (Персональный ключ доступа):
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="patInput"
                  value={pat}
                  onChange={(e) => setPat(e.target.value)}
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="repoUrlInput">
                  <FontAwesomeIcon icon={faLink} className="me-2" />
                  Ссылка на GitHub репозиторий:
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="repoUrlInput"
                  value={repoUrl}
                  onChange={(e) => setRepoUrl(e.target.value)}
                  required // обязательное поле
                />
              </div>
            </>
          )}
          {inputType === "file" && (
            <div className="form-group">
              <label htmlFor="codeFileInput">
                <FontAwesomeIcon icon={faUpload} className="me-2" />
                Загрузить архив:
              </label>
              <input
                type="file"
                className="form-control-file"
                id="codeFileInput"
                onChange={(e) => setFile(e.target.files[0])}
                required // обязательное поле
              />
            </div>
          )}
          <button
            type="submit"
            className="btn btn-primary mt-3 align-self-stretch mx-5"
            disabled={loading}
          >
            {loading ? (
              <>
                <FontAwesomeIcon icon={faSpinner} spin className="me-2" />
                Загрузка...
              </>
            ) : (
              <>
                <FontAwesomeIcon icon={faPlay} className="me-2" />
                Генерировать
              </>
            )}
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
