import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Swal from "sweetalert2";
import {
  faUser,
  faEnvelope,
  faClock,
  faIdCard,
  faArrowRight,
  faPlus,
  faPlusCircle,
  faLock,
  faUsers,
} from "@fortawesome/free-solid-svg-icons";
import { Modal, Button } from "react-bootstrap";
import { getRoleStr, Roles } from "../../../Utils/Roles";
import {
  fetchAllUsersInfo,
  makeAuthenticatedRequest,
  createUser,
} from "../../../Utils/ApiService";
import "./UsersPage.css";

const locale = "ru-RU";

function UsersPage({ handleLogout }) {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchQuery, setSearchQuery] = useState("");
  const [showCreateUserModal, setShowCreateUserModal] = useState(false); // Состояние для модального окна создания нового пользователя
  const [newUser, setNewUser] = useState({
    name: "",
    email: "",
    roleId: Roles.USER,
    password: "",
  });

  const navigate = useNavigate();

  const loadUsers = async () => {
    try {
      const usersData = await makeAuthenticatedRequest(fetchAllUsersInfo);
      setUsers(usersData);
    } catch (err) {
      setError(
        `Ошибка при получении информации о пользователях: ${err.response.data}`
      );
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadUsers();
  }, []);

  const handleMoreInfoClick = (userId) => {
    navigate(`/profile/${userId}`);
  };

  // Обработчик изменения данных нового пользователя
  const handleNewUserChange = (e) => {
    const { name, value } = e.target;
    setNewUser((prevUser) => ({ ...prevUser, [name]: value }));
  };

  // Обработчик отправки формы для создания нового пользователя
  const handleCreateUserSubmit = async (e) => {
    e.preventDefault();
    try {
      await makeAuthenticatedRequest(() => createUser(newUser));

      setShowCreateUserModal(false);

      Swal.fire({
        icon: "success",
        title: "Успешно!",
        text: `Пользователь "${newUser.name}" успешно создан.`,
      });

      setNewUser({
        name: "",
        email: "",
        roleId: Roles.USER,
        password: "",
      });
      await loadUsers();
      setShowCreateUserModal(false);
    } catch (error) {
      console.error("Ошибка при создании пользователя:", error);
      Swal.fire({
        icon: "error",
        title: "Ошибка",
        text: `Ошибка при создании пользователя "${newUser.name}": ${error.response.data}`,
      });
    }
  };

  // Открытие модального окна для создания нового пользователя
  const handleOpenCreateUserModal = () => {
    setShowCreateUserModal(true);
  };

  // Закрытие модального окна для создания нового пользователя
  const handleCloseCreateUserModal = () => {
    setShowCreateUserModal(false);
  };

  // Обработка поискового запроса
  const handleSearchChange = (e) => {
    setSearchQuery(e.target.value);
  };

  // Фильтрация списка пользователей по поисковому запросу
  const filteredUsers = users.filter((user) =>
    user.email.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <div className="users-page">
      <div className="text-center">
        <h1>
          <FontAwesomeIcon icon={faUsers} style={{ marginRight: "15px" }} />
          Пользователи
        </h1>
        {/* Кнопка "Создать" */}
        <Button
          variant="primary"
          className="mb-3"
          onClick={handleOpenCreateUserModal}
        >
          <FontAwesomeIcon icon={faPlusCircle} /> Создать
        </Button>
      </div>

      {/* Поле поиска */}
      <input
        type="text"
        placeholder="Поиск по email..."
        value={searchQuery}
        onChange={handleSearchChange}
        className="form-control mb-3"
        style={{ width: "400px", margin: "0 auto" }}
      />
      <hr></hr>
      {/* Модальное окно для создания нового пользователя */}
      <Modal
        show={showCreateUserModal}
        onHide={handleCloseCreateUserModal}
        centered
      >
        <Modal.Header closeButton className="w-100">
          <Modal.Title className="w-100 text-center">
            Создать пользователя
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <form onSubmit={handleCreateUserSubmit}>
            <div className="form-group">
              <label>
                <FontAwesomeIcon icon={faUser} style={{ marginRight: "5px" }} />
                Имя:
              </label>
              <input
                type="text"
                name="name"
                value={newUser.name}
                onChange={handleNewUserChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <label>
                <FontAwesomeIcon
                  icon={faEnvelope}
                  style={{ marginRight: "5px" }}
                />
                Email:
              </label>
              <input
                type="email"
                name="email"
                value={newUser.email}
                onChange={handleNewUserChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <label>
                <FontAwesomeIcon
                  icon={faIdCard}
                  style={{ marginRight: "5px" }}
                />
                Роль:
              </label>
              <select
                name="roleId"
                value={newUser.roleId}
                onChange={handleNewUserChange}
                className="form-control"
                required
              >
                <option value={Roles.ADMINISTRATOR}>Администратор</option>
                <option value={Roles.USER}>Пользователь</option>
                <option value={Roles.GUEST}>Гость</option>
              </select>
            </div>
            <div className="form-group">
              <label>
                <FontAwesomeIcon icon={faLock} style={{ marginRight: "5px" }} />
                Пароль:
              </label>
              <input
                type="password"
                name="password"
                value={newUser.password}
                onChange={handleNewUserChange}
                className="form-control"
                required
              />
            </div>
            <div
              className="form-group text-center"
              style={{ marginTop: "15px" }}
            >
              <Button type="submit" variant="primary">
                Создать
              </Button>
            </div>
          </form>
        </Modal.Body>
      </Modal>

      {/* Список пользователей */}
      {loading && <p>Загрузка пользователей...</p>}
      {error && (
        <p className="error">
          Произошла ошибка при загрузке пользователей: {error}
        </p>
      )}
      {!loading && !error && (
        <ul className="list-group">
          {filteredUsers.map((user) => (
            <li
              className="list-group-item user-item"
              key={user.id}
              style={{
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
              }}
            >
              <div className="profile-info">
                <div className="profile-info-row">
                  <strong>
                    <FontAwesomeIcon icon={faUser} /> Имя:
                  </strong>
                  <span>{user.name}</span>
                </div>
                <div className="profile-info-row">
                  <strong>
                    <FontAwesomeIcon icon={faEnvelope} /> Email:
                  </strong>
                  <span>{user.email}</span>
                </div>
                <div class="profile-info-row">
                  <strong>
                    <FontAwesomeIcon icon={faClock} /> Дата создания:
                  </strong>
                  <span>
                    {new Date(user.createdAt).toLocaleDateString(locale)}
                  </span>
                </div>
                <div className="profile-info-row">
                  <strong>
                    <FontAwesomeIcon icon={faIdCard} /> Роль:
                  </strong>
                  <span>{getRoleStr(user.roleId)}</span>
                </div>
              </div>

              <button
                className="btn btn-primary text-center"
                onClick={() => handleMoreInfoClick(user.id)}
                style={{ marginTop: "10px" }}
              >
                <FontAwesomeIcon
                  icon={faArrowRight}
                  style={{ marginRight: "5px" }}
                />
                Подробнее
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default UsersPage;
