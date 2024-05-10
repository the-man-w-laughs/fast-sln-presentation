import React, { useEffect, useState } from "react";
import { fetchPlans } from "../../../Utils/ApiService";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faListAlt,
  faCalendarAlt,
  faInfoCircle,
  faRubleSign,
} from "@fortawesome/free-solid-svg-icons";
import "./Plans.css";

const Plans = () => {
  const [plans, setPlans] = useState([]);

  useEffect(() => {
    async function loadPlans() {
      try {
        const plansData = await fetchPlans();

        if (plansData) {
          setPlans(plansData);
        } else {
          console.warn("No plans data found.");
        }
      } catch (error) {
        console.error("Error fetching plans data:", error);
      } finally {
      }
    }

    loadPlans();
  }, []);

  return (
    <div className="plans-container">
      <h2>
        Цены на подписку <FontAwesomeIcon icon={faRubleSign} />
      </h2>
      <p>Мы предлагаем несколько планов подписки:</p>
      <div className="plans-row">
        {plans.map((plan) => (
          <div className="plan-box" key={plan.id}>
            <div className="card">
              <div className="card-body">
                <h5 className="card-title title">{plan.name}</h5>
                <hr></hr>
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
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Plans;
