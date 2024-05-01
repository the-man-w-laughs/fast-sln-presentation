import React from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { Carousel } from "react-bootstrap";

const HomePage = () => {
  return (
    <div className="container my-5">
      <header className="bg-primary text-white p-4 mb-4">
        <h1>Добро пожаловать на наш сервис для генерации диаграмм и схем!</h1>
        <p>Наше приложение предназначено для визуализации кода на языке C#.</p>
      </header>

      <main>
        {/* Section for Pricing */}
        <section className="mb-5">
          <h2>Цены на подписку</h2>
          <p>Мы предлагаем несколько планов подписки:</p>
          <ul>
            <li>
              <strong>Базовый:</strong> $10/месяц
            </li>
            <li>
              <strong>Профессиональный:</strong> $25/месяц
            </li>
            <li>
              <strong>Премиум:</strong> $50/месяц
            </li>
          </ul>
        </section>

        {/* Section for App Description and Features */}
        <section className="mb-5">
          <h2>Описание приложения и его возможностей</h2>
          <p>
            Наше веб-приложение позволяет визуализировать код на языке C# в виде
            диаграмм и схем, что помогает разработчикам и другим
            заинтересованным лицам лучше понять структуру программы и связи
            между классами.
          </p>
          <ul>
            <li>Генерация диаграмм классов</li>
            <li>Генерания блок-схем</li>
            <li>Экспорт результатов работы программы</li>
          </ul>
        </section>

        {/* Section for Visual Demo */}
        <section className="mb-5">
          <h2>Визуальная демонстрация</h2>
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

        {/* Contact Information */}
        <section>
          <h2>Связь с поддержкой</h2>
          <p>
            Если у вас возникли вопросы или предложения, свяжитесь с нами по
            электронной почте:{" "}
            <a href="mailto:fastslnpresentationsupport@gmail.com">
              fastslnpresentationsupport@gmail.com
            </a>
          </p>
        </section>
      </main>

      <footer className="bg-light text-center p-4 mt-5">
        <p>© 2024 fast-sln-presentation</p>
      </footer>
    </div>
  );
};

export default HomePage;
