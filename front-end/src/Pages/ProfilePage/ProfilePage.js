import React, { useState, useEffect } from "react";
import {
  fetchActiveSubscriptionByToken,
  fetchUserSubscriptionsByToken,
  getUserDataByToken,
  fetchPlans,
  makeAuthenticatedRequest,
} from "../../Utils/ApiService";
import { useNavigate } from "react-router-dom";
import { getRoleStr } from "../../Utils/Roles";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faSignOutAlt,
  faUser,
  faEnvelope,
  faClock,
  faIdCard,
  faList,
  faExclamationTriangle,
  faCalendarAlt,
  faInfoCircle,
  faTags,
} from "@fortawesome/free-solid-svg-icons";
import "bootstrap/dist/css/bootstrap.min.css";
import "./ProfilePage.css"; // Подключите файл стилей для страницы профиля

const locale = "ru-RU";

function ProfilePage({ handleLogout }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [activeSubscription, setActiveSubscription] = useState(null);
  const [subscriptions, setSubscriptions] = useState([]);
  const [plans, setPlans] = useState([]); // Состояние для хранения списка планов
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const userData = await makeAuthenticatedRequest(getUserDataByToken);
        setUser(userData);

        // Получить активную подписку пользователя
        const activeSub = await makeAuthenticatedRequest(
          fetchActiveSubscriptionByToken
        );
        setActiveSubscription(activeSub);

        // Получить все подписки пользователя
        const userSubs = await makeAuthenticatedRequest(
          fetchUserSubscriptionsByToken
        );
        setSubscriptions(userSubs);

        // Получить список планов
        const planList = await fetchPlans();
        setPlans(planList);
      } catch (error) {
        console.error("Ошибка при загрузке данных:", error);
        onLogout();
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  // Функция для получения названия плана по его ID
  const getPlanNameById = (planId) => {
    const plan = plans.find((p) => p.id === planId);
    return plan ? plan.name : "Название плана не найдено";
  };

  const onLogout = () => {
    handleLogout();
    navigate("/home");
  };

  if (loading) {
    return <p>Загрузка данных...</p>;
  }

  if (!user) {
    return <p>Ошибка загрузки данных о пользователе</p>;
  }

  return (
    <div className="container profile-container">
      <div className="row justify-content-center">
        <div className="col-md-8">
          <div className="card profile-card">
            <div
              className="card-header text-center"
              style={{
                backgroundColor: "#121F3B",
              }}
            >
              <h4 className="profile-header">Профиль</h4>
            </div>
            <div className="card-body">
              <p className="profile-info">
                <strong>
                  <FontAwesomeIcon icon={faUser} /> Имя:
                </strong>{" "}
                {user.name}
              </p>
              <p className="profile-info">
                <strong>
                  <FontAwesomeIcon icon={faEnvelope} /> Email:
                </strong>{" "}
                {user.email}
              </p>
              <p className="profile-info">
                <strong>
                  <FontAwesomeIcon icon={faClock} /> Дата создания:
                </strong>{" "}
                {new Date(user.createdAt).toLocaleDateString(locale)}
              </p>
              <p className="profile-info">
                <strong>
                  <FontAwesomeIcon icon={faIdCard} /> Роль:
                </strong>{" "}
                {getRoleStr(user.roleId)}
              </p>

              <hr />

              {activeSubscription ? (
                <div className="active-subscription mt-4">
                  <h5 className="profile-subtitle">Активная подписка:</h5>
                  <ul className="list-group">
                    <li className="list-group-item">
                      <strong>
                        <FontAwesomeIcon icon={faTags} /> План:
                      </strong>{" "}
                      {getPlanNameById(activeSubscription.planId)} <br />
                      <strong>
                        <FontAwesomeIcon icon={faCalendarAlt} /> Дата начала:
                      </strong>{" "}
                      {new Date(
                        activeSubscription.startDate
                      ).toLocaleDateString(locale)}{" "}
                      <br />
                      <strong>
                        <FontAwesomeIcon icon={faCalendarAlt} /> Дата окончания:
                      </strong>{" "}
                      {new Date(activeSubscription.endDate).toLocaleDateString(
                        locale
                      )}
                    </li>
                  </ul>
                </div>
              ) : (
                <div className="alert alert-danger" role="alert">
                  <FontAwesomeIcon icon={faExclamationTriangle} /> У
                  пользователя нет активных подписок.
                </div>
              )}
              <hr />
              <div className="all-subscriptions mt-4">
                <h5 className="profile-subtitle">Все подписки:</h5>
                {subscriptions && subscriptions.length > 0 ? (
                  <div style={{ maxHeight: "300px", overflowY: "scroll" }}>
                    <ul className="list-group list-group-flush">
                      {subscriptions.map((subscription, index) => (
                        <li
                          key={index}
                          className="list-group-item subscription-item"
                        >
                          <strong>
                            <FontAwesomeIcon icon={faTags} /> План:
                          </strong>{" "}
                          {getPlanNameById(subscription.planId)} <br />
                          <strong>
                            <FontAwesomeIcon icon={faCalendarAlt} /> Дата
                            начала:
                          </strong>{" "}
                          {new Date(subscription.startDate).toLocaleDateString(
                            locale
                          )}{" "}
                          <br />
                          <strong>
                            <FontAwesomeIcon icon={faCalendarAlt} /> Дата
                            окончания:
                          </strong>{" "}
                          {new Date(subscription.endDate).toLocaleDateString(
                            locale
                          )}
                        </li>
                      ))}
                    </ul>
                  </div>
                ) : (
                  <div className="alert alert-danger" role="alert">
                    <FontAwesomeIcon icon={faList} /> У пользователя нет
                    подписок.
                  </div>
                )}
              </div>
            </div>
            <div className="card-footer text-center">
              <button className="btn btn-danger" onClick={onLogout}>
                <FontAwesomeIcon icon={faSignOutAlt} /> Выйти
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ProfilePage;
