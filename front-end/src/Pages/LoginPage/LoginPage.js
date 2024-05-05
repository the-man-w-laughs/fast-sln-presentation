import React, { useState } from "react";
import "./LoginPage.css";
import {
  setAccessToken,
  getAccessToken,
  setUserInfo,
  getUserInfo,
  setRefreshToken,
} from "../../Utils/LocalStorage";
import { useNavigate } from "react-router-dom";

import { login } from "../../Utils/ApiService";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faEnvelope,
  faLock,
  faSignInAlt,
} from "@fortawesome/free-solid-svg-icons";

function LoginPage({ setUserInfo }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const data = await login(email, password);

      // Сохраняем access token в локальном хранилище
      setAccessToken(data.accessToken);
      setRefreshToken(data.refreshToken);
      setUserInfo(data.user);
      console.log("Вход выполнен успешно! Токен доступа:", data.accessToken);
      navigate("/profile");
    } catch (error) {
      setError("Произошла ошибка во время процесса входа. Попробуйте еще раз.");
    }
  };

  return (
    <div className="d-flex justify-content-center align-items-center margin-top-20">
      <form
        onSubmit={handleLogin}
        className="border p-4 rounded fixed-width-form"
      >
        <h2 className="text-center">Вход</h2>
        {error && <div className="alert alert-danger text-center">{error}</div>}
        <div className="mb-3">
          <label className="form-label">
            <FontAwesomeIcon icon={faEnvelope} className="me-2" />
            Электронная почта
          </label>
          <input
            type="email"
            className="form-control"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label className="form-label">
            <FontAwesomeIcon icon={faLock} className="me-2" />
            Пароль
          </label>
          <input
            type="password"
            className="form-control"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <button type="submit" className="btn btn-primary w-100">
          <FontAwesomeIcon icon={faSignInAlt} className="me-2" />
          Войти
        </button>
      </form>
    </div>
  );
}

export default LoginPage;
