import React from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import Plans from "../../components/SoftComponents/PlansConponent/Plans";
import { Carousel } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faEnvelope,
  faCogs,
  faChartBar,
  faPhoneAlt,
  faDiagramProject,
  faFileExport,
  faCheck,
  faCheckCircle,
  faCheckSquare,
} from "@fortawesome/free-solid-svg-icons";

const HomePage = () => {
  return (
    <>
      <header
        style={{
          backgroundColor: "#121F3B",
          color: "white",
          padding: "1rem",
          marginBottom: "1rem",
        }}
      >
        <h1 style={{ textAlign: "center" }}>
          Добро пожаловать на наш сервис для генерации диаграмм и схем!
        </h1>
        <p style={{ textAlign: "center" }}>
          Наше приложение предназначено для визуализации кода на языке C#.
        </p>
      </header>
      <div className="container my-0">
        <main>
          {/* Section for App Description and Features */}
          <section className="mb-5">
            <h2 className="text-center">
              Описание приложения и его возможностей{" "}
              <FontAwesomeIcon icon={faCogs} />
            </h2>
            <p className="text-center">
              Наше веб-приложение позволяет визуализировать код на языке C# в
              виде диаграмм и схем, что помогает разработчикам и другим
              заинтересованным лицам лучше понять структуру программы и связи
              между классами.
            </p>
            <ul style={{ listStyleType: "none", paddingLeft: 0 }}>
              <li>
                <FontAwesomeIcon icon={faCheckSquare} /> Генерация диаграмм
                классов
              </li>
              <li>
                <FontAwesomeIcon icon={faCheckSquare} /> Генерация блок-схем
              </li>
              <li>
                <FontAwesomeIcon icon={faCheckSquare} /> Экспорт результатов
                работы программы
              </li>
            </ul>
          </section>
          <hr />
          {/* Section for Visual Demo */}
          <section className="mb-5">
            <h2 className="text-center">Визуальная демонстрация</h2>
            <Carousel>
              <Carousel.Item>
                <img
                  className="d-block w-100"
                  src="images/class-diagram-example-1.png"
                  alt="Демонстрация 1"
                />
                <Carousel.Caption>
                  <p>Демонстрация визуализации кода</p>
                </Carousel.Caption>
              </Carousel.Item>
              <Carousel.Item>
                <img
                  className="d-block w-100"
                  src="images/flowchart-example-1.png"
                  alt="Демонстрация 2"
                />
                <Carousel.Caption>
                  <p>Демонстрация диаграмм и схем</p>
                </Carousel.Caption>
              </Carousel.Item>
            </Carousel>
          </section>
          <hr />
          {/* Section for Pricing */}
          <section className="mb-5">
            <Plans />
          </section>
          <hr />
          {/* Contact Information */}
          <section>
            <h2 className="text-center">
              Связь с поддержкой <FontAwesomeIcon icon={faEnvelope} />
            </h2>
            <p className="text-center">
              Если у вас возникли вопросы или предложения, свяжитесь с нами по
              электронной почте:{" "}
              <a href="mailto:fastslnpresentationsupport@gmail.com">
                fastslnpresentationsupport@gmail.com{" "}
              </a>
            </p>
          </section>
        </main>
      </div>
    </>
  );
};

export default HomePage;
