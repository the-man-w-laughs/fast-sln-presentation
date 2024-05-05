import React, { useState, useEffect } from "react";
import Swal from "sweetalert2";
import {
  fetchPlans,
  deletePlan,
  createPlan,
  makeAuthenticatedRequest,
} from "../../../Utils/ApiService";
import {
  faCalendarAlt,
  faInfoCircle,
  faRubleSign,
  faTrash,
  faPlusCircle,
  faPencil,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Button, Modal } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "./PlansPage.css";

function PlansPage({ handleLogout }) {
  const [plans, setPlans] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [newPlan, setNewPlan] = useState({
    name: "",
    duration: 0,
    price: 0,
    description: "",
  });

  const [showModal, setShowModal] = useState(false);

  // Загружаем планы при монтировании компонента
  useEffect(() => {
    const loadPlans = async () => {
      try {
        const plansData = await fetchPlans();
        setPlans(plansData);
        setLoading(false);
      } catch (err) {
        setError(err);
        setLoading(false);
      }
    };

    loadPlans();
  }, []);

  const handleDeletePlan = async (planId) => {
    const planToDelete = plans.find((plan) => plan.id === planId);

    const result = await Swal.fire({
      title: `Вы уверены, что хотите удалить план "${planToDelete.name}"?`,
      text: "Этот план будет удален. Вы не сможете восстановить его!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Да, удалить",
      cancelButtonText: "Отмена",
    });

    if (result.isConfirmed) {
      try {
        await makeAuthenticatedRequest(deletePlan, planId);
        setPlans((prevPlans) => prevPlans.filter((plan) => plan.id !== planId));
        Swal.fire({
          icon: "success",
          title: "Удалено!",
          text: `План "${planToDelete.name}" успешно удален.`,
        });
      } catch (error) {
        console.error("Ошибка при удалении плана:", error);
        Swal.fire({
          icon: "error",
          title: "Ошибка",
          text: `Ошибка при удалении плана "${planToDelete.name}": ${error.response.data}`,
        });
      }
    }
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setNewPlan({ ...newPlan, [name]: value });
  };

  const handleFormSubmit = async (event) => {
    event.preventDefault();
    try {
      const createdPlan = await makeAuthenticatedRequest(createPlan, newPlan);
      setPlans((prevPlans) => [...prevPlans, createdPlan]);
      setNewPlan({
        name: "",
        duration: 0,
        price: 0,
        description: "",
      });

      Swal.fire({
        icon: "success",
        title: "Успешно!",
        text: `План "${createdPlan.name}" успешно создан.`,
      });
      setShowModal(false);
    } catch (error) {
      console.error("Ошибка при создании плана:", error);
      Swal.fire({
        icon: "error",
        title: "Ошибка",
        text: `Ошибка при создании плана "${newPlan.name}": ${error.response.data}`,
      });
    }
  };

  const handleShowModal = () => {
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
  };

  if (loading) {
    return <p>Загрузка планов...</p>;
  }

  if (error) {
    return (
      <p className="error">
        Произошла ошибка при загрузке планов: {error.message}
      </p>
    );
  }

  return (
    <div className="plans-page">
      <Modal show={showModal} onHide={handleCloseModal} centered>
        <Modal.Header closeButton className="w-100">
          <Modal.Title className="w-100 text-center">Создать план</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <form onSubmit={handleFormSubmit}>
            <div className="form-group">
              <label>
                <FontAwesomeIcon icon={faPencil} /> Название:
              </label>
              <input
                type="text"
                name="name"
                value={newPlan.name}
                onChange={handleInputChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <label>
                <FontAwesomeIcon icon={faCalendarAlt} /> Продолжительность
                (дни):
              </label>
              <input
                type="number"
                name="duration"
                value={newPlan.duration}
                onChange={handleInputChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <label>
                <FontAwesomeIcon icon={faRubleSign} /> Цена:
              </label>
              <input
                type="number"
                name="price"
                value={newPlan.price}
                onChange={handleInputChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <label>
                <FontAwesomeIcon icon={faInfoCircle} /> Описание:
              </label>
              <textarea
                name="description"
                value={newPlan.description}
                onChange={handleInputChange}
                className="form-control"
                style={{ height: "100px" }}
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
      <div className="text-center">
        <h1>
          <FontAwesomeIcon icon={faRubleSign} style={{ marginRight: "15px" }} />
          Планы
        </h1>
        <Button variant="primary" onClick={handleShowModal}>
          <FontAwesomeIcon icon={faPlusCircle} /> Добавить план
        </Button>
      </div>
      <hr />
      <ul className="list-group mt-4">
        {plans.map((plan) => (
          <div className="plan-box plan-item" key={plan.id}>
            <div className="card">
              <div className="card-body">
                <h5 className="card-title title text-center">{plan.name}</h5>
                <hr />
                <p className="card-text">
                  <FontAwesomeIcon icon={faCalendarAlt} /> Период:{" "}
                  {plan.duration} дней
                </p>
                {plan.description && (
                  <p className="card-text">
                    <FontAwesomeIcon icon={faInfoCircle} /> Описание:{" "}
                    {plan.description}
                  </p>
                )}
                <div className="price">
                  <FontAwesomeIcon icon={faRubleSign} /> Цена: {plan.price} р.
                </div>
                <hr />
                <button
                  className="btn btn-danger"
                  onClick={() => handleDeletePlan(plan.id)}
                  style={{ display: "block", margin: "0 auto" }}
                >
                  <FontAwesomeIcon icon={faTrash} /> Удалить
                </button>
              </div>
            </div>
          </div>
        ))}
      </ul>
    </div>
  );
}

export default PlansPage;
