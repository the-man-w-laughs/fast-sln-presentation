import React, { useState, useEffect } from "react";
import Swal from "sweetalert2";
import {
  fetchActiveSubscriptionByToken,
  fetchUserSubscriptionsByToken,
  getUserDataByToken,
  fetchPlans,
  makeAuthenticatedRequest,
  getUserDataById,
  fetchActiveSubscriptionById,
  fetchUserSubscriptionsById,
  deleteUserSubscriptionById,
  deleteUser,
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
  faTrashAlt,
} from "@fortawesome/free-solid-svg-icons";
import { useParams } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "./ProfilePage.css"; // Подключите файл стилей для страницы профиля

const locale = "ru-RU";

function ProfilePage({ handleLogout }) {
  const { id } = useParams();
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [activeSubscription, setActiveSubscription] = useState(null);
  const [subscriptions, setSubscriptions] = useState([]);
  const [plans, setPlans] = useState([]); // Состояние для хранения списка планов
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        let userData;
        let activeSub;
        let userSubs;

        if (id !== undefined) {
          userData = await makeAuthenticatedRequest(getUserDataById, id).catch(
            () => {}
          );
          activeSub = await makeAuthenticatedRequest(
            fetchActiveSubscriptionById,
            id
          ).catch(() => {});
          userSubs = await makeAuthenticatedRequest(
            fetchUserSubscriptionsById,
            id
          ).catch(() => {});
        } else {
          userData = await makeAuthenticatedRequest(getUserDataByToken).catch(
            () => {}
          );
          activeSub = await makeAuthenticatedRequest(
            fetchActiveSubscriptionByToken
          ).catch(() => {});
          userSubs = await makeAuthenticatedRequest(
            fetchUserSubscriptionsByToken
          ).catch(() => {});
        }

        setUser(userData);
        setActiveSubscription(activeSub);
        setSubscriptions(userSubs);

        const planList = await fetchPlans();
        setPlans(planList);
      } catch (error) {
        console.error("Ошибка при загрузке данных:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  // Функция для получения названия плана по его ID
  const getPlanNameById = (planId) => {
    const plan = plans.find((p) => p.id === planId);
    return plan ? plan.name : "Название плана не найдено";
  };

  const onLogout = async () => {
    const result = await Swal.fire({
      title: "Вы уверены?",
      text: "Вы действительно хотите выйти?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Да, выйти",
      cancelButtonText: "Отмена",
    });

    // Если пользователь подтвердил выход, вызываем функцию handleLogout и переходим на домашнюю страницу
    if (result.isConfirmed) {
      handleLogout();
      navigate("/home");
    }
  };

  const handleDeleteSubscription = async (subscriptionId) => {
    try {
      const result = await Swal.fire({
        title: "Вы уверены?",
        text: "Вы действительно хотите удалить подписку?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Да, удалить",
        cancelButtonText: "Отмена",
      });

      if (result.isConfirmed) {
        await makeAuthenticatedRequest(
          deleteUserSubscriptionById,
          subscriptionId
        );

        setSubscriptions((prevSubscriptions) =>
          prevSubscriptions.filter(
            (subscription) => subscription.id !== subscriptionId
          )
        );

        const activeSub = await makeAuthenticatedRequest(
          fetchActiveSubscriptionById,
          id
        );
        setActiveSubscription(activeSub);

        Swal.fire("Удалено!", "Подписка была успешно удалена.", "success");
      }
    } catch (error) {
      console.error("Ошибка при удалении подписки:", error);
      Swal.fire(
        "Ошибка!",
        "Произошла ошибка при удалении подписки. Пожалуйста, попробуйте позже.",
        "error"
      );
    }
  };

  const handleDeleteUser = async () => {
    try {
      const result = await Swal.fire({
        title: "Вы уверены?",
        text: "Вы действительно хотите удалить пользователя?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Да, удалить",
        cancelButtonText: "Отмена",
      });

      if (result.isConfirmed) {
        var deletedUser = await makeAuthenticatedRequest(deleteUser, id);

        if (deletedUser) {
          Swal.fire("Удалено!", "Пользователь был успешно удален.", "success");
          navigate("/admin"); // Перенаправить на домашнюю страницу после удаления
        } else {
          throw new Error("Ошибка при удалении пользователя!");
        }
      }
    } catch (error) {
      console.error("Ошибка при удалении пользователя:", error);
      Swal.fire(
        "Ошибка!",
        `Произошла ошибка при удалении пользователя: ${error.response.data}`,
        "error"
      );
    }
  };

  if (loading) {
    return <p>Загрузка данных...</p>;
  }

  if (!user) {
    return <p className="text-center">Ошибка загрузки данных о пользователе</p>;
  }

  return (
    <div className="container profile-container">
      <div className="row justify-content-center">
        <div className="col-md-8">
          <div className="card profile-card">
            <div
              className="card-header text-center"
              style={{ backgroundColor: "#121F3B" }}
            >
              <h4 className="profile-header">Профиль</h4>
            </div>
            <div className="card-body">
              <p>
                <strong>
                  <FontAwesomeIcon icon={faUser} /> Имя:{" "}
                </strong>{" "}
                {user.name}
              </p>
              <p>
                <strong>
                  <FontAwesomeIcon icon={faEnvelope} /> Email:
                </strong>{" "}
                {user.email}
              </p>
              <p>
                <strong>
                  <FontAwesomeIcon icon={faClock} /> Дата создания:
                </strong>{" "}
                {new Date(user.createdAt).toLocaleDateString(locale)}
              </p>
              <p>
                <strong>
                  <FontAwesomeIcon icon={faIdCard} /> Роль:
                </strong>{" "}
                {getRoleStr(user.roleId)}
              </p>
              <hr />
              <h5 className="profile-subtitle text-center">
                Активная подписка
              </h5>
              {activeSubscription ? (
                <div className="active-subscription mt-4">
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
                <h5 className="profile-subtitle text-center">Все подписки</h5>
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
                          )}{" "}
                          <br></br>
                          {id != null && (
                            <button
                              className="btn btn-danger btn-sm mt-2"
                              onClick={() =>
                                handleDeleteSubscription(subscription.id)
                              }
                            >
                              <FontAwesomeIcon icon={faTrashAlt} /> Удалить
                            </button>
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
              {id == null && (
                <button className="btn btn-danger" onClick={onLogout}>
                  <FontAwesomeIcon icon={faSignOutAlt} /> Выйти
                </button>
              )}
              {id != null && (
                <button
                  className="btn btn-danger ml-2"
                  onClick={handleDeleteUser}
                >
                  <FontAwesomeIcon icon={faTrashAlt} /> Удалить пользователя
                </button>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ProfilePage;
