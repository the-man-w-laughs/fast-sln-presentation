import axios from "axios";
import {
  getAccessToken,
  getRefreshToken,
  setRefreshToken,
  setAccessToken,
} from "./LocalStorage";
import Mutex from "./Mutex";

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
    throw error;
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
    throw error;
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
    throw error;
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

async function refreshToken() {
  try {
    // Получите текущие токены доступа и обновления из вашего хранилища
    const accessToken = getAccessToken();
    const refreshToken = getRefreshToken();

    // Подготовьте тело запроса
    const requestBody = {
      accessToken: accessToken,
      refreshToken: refreshToken,
    };

    // Отправьте запрос к маршруту `/refresh-token`
    const response = await axios.post(
      `${BASE_URL}/refresh-token`,
      requestBody,
      {
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    // Если запрос прошел успешно, обновите токены в хранилище
    if (response.status === 200) {
      const newAccessToken = response.data.accessToken;
      const newRefreshToken = response.data.refreshToken;

      // Сохраните новые токены
      setAccessToken(newAccessToken);
      setRefreshToken(newRefreshToken);

      // Верните новые токены, если нужно использовать их в дальнейшем
      return { accessToken: newAccessToken, refreshToken: newRefreshToken };
    } else {
      throw new Error(`HTTP error: ${response.status}`);
    }
  } catch (error) {
    console.error("Ошибка при обновлении токена:", error);
    throw error;
  }
}

const mutex = new Mutex();

async function makeAuthenticatedRequest(requestFunction, ...args) {
  const release = await mutex.lock();
  try {
    // Execute the request function with the given arguments
    const response = await requestFunction(...args);
    return response;
  } catch (error) {
    if (error.response && error.response.status === 401) {
      console.log("Токен истёк, попытка обновить...");
      try {
        // Try refreshing the token
        const tokens = await refreshToken();
        const newAccessToken = tokens.accessToken;

        // Retry the request with the new access token
        const updatedArgs = args.map((arg) =>
          arg === null ? newAccessToken : arg
        );
        const response = await requestFunction(...updatedArgs);
        return response;
      } catch (refreshError) {
        throw refreshError;
      }
    } else {
      console.error("Ошибка при запросе:", error);
    }
  } finally {
    release();
  }
}

export {
  makeAuthenticatedRequest,
  getUserDataByToken,
  fetchPlans,
  login,
  fetchActiveSubscriptionByToken,
  fetchUserSubscriptionsByToken,
  generateClassDiagramByGithub,
  generateClassDiagramByFile,
  generateFlowChartByCode,
  refreshToken,
};
