import axios from "axios";
import { getAccessToken } from "./LocalStorage";
const BASE_URL = "http://localhost:5137";

async function getUserDataByToken(token = null) {
  try {
    if (token == null) token = getAccessToken();
    const response = await axios.get(`${BASE_URL}/users/token`, {
      headers: {
        Accept: "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    return response.data;
  } catch (error) {
    throw new Error(`Error fetching user data: ${error.message}`);
  }
}

async function fetchPlans() {
  try {
    const response = await axios.get(`${BASE_URL}/plans`);
    return response.data;
  } catch (error) {
    console.error("Ошибка при загрузке планов:", error);
  }
}

async function login(email, password) {
  const requestBody = {
    email: email,
    password: password,
  };
  try {
    const response = await axios.post(`${BASE_URL}/login`, requestBody, {
      headers: {
        "Content-Type": "application/json",
      },
    });

    // Return the response data
    return response.data;
  } catch (error) {
    // Handle errors
    throw new Error(
      `Error during login: ${error.response?.data?.message || error.message}`
    );
  }
}

async function fetchActiveSubscriptionByToken(token) {
  try {
    if (token == null) token = getAccessToken();
    const response = await axios.get(`${BASE_URL}/subscriptions/active/token`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при получении активной подписки пользователя:",
      error
    );
  }
}

async function fetchUserSubscriptionsByToken(token) {
  try {
    if (token == null) token = getAccessToken();
    const response = await axios.get(`${BASE_URL}/subscriptions/token`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error("Ошибка при получении подписок пользователя:", error);
  }
}

async function generateClassDiagramByGithub(
  pat,
  author,
  repoName,
  token = null
) {
  if (token == null) token = getAccessToken();

  const requestBody = {
    pat: pat,
    owner: author,
    repoName: repoName,
  };
  const url = `${BASE_URL}/class-diagram/github`;

  try {
    const response = await axios.post(url, requestBody, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.status === 200) {
      return response.data; // Return JSON data
    } else {
      throw new Error(`HTTP error: ${response.status}`);
    }
  } catch (error) {
    console.error("Ошибка при генерации диаграммы классов:", error);
    throw error;
  }
}

async function generateClassDiagramByFile(file, token = null) {
  if (token == null) token = getAccessToken();

  const requestBody = new FormData();
  requestBody.append("file", file);
  const url = `${BASE_URL}/class-diagram/zip-file`;

  try {
    const response = await axios.post(url, requestBody, {
      headers: {
        "Content-Type": "multipart/form-data",
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.status === 200) {
      return response.data; // Return JSON data
    } else {
      throw new Error(`HTTP error: ${response.status}`);
    }
  } catch (error) {
    console.error("Ошибка при генерации диаграммы классов:", error);
    throw error;
  }
}

async function generateFlowChartByCode(code, token = null) {
  if (token == null) token = getAccessToken();

  const requestBody = JSON.stringify(code);
  const url = `${BASE_URL}/flowchart`;

  try {
    const response = await axios.post(url, requestBody, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.status === 200) {
      return response.data; // Return JSON data
    } else {
      throw new Error(`HTTP error: ${response.status}`);
    }
  } catch (error) {
    console.error("Ошибка при генерации блок-схемы:", error);
    throw error;
  }
}

export {
  getUserDataByToken,
  fetchPlans,
  login,
  fetchActiveSubscriptionByToken,
  fetchUserSubscriptionsByToken,
  generateClassDiagramByGithub,
  generateClassDiagramByFile,
  generateFlowChartByCode,
};
