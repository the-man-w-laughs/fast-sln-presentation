import axios from "axios";
import {
  getAccessToken,
  getRefreshToken,
  setAccessToken,
  setRefreshToken,
} from "./LocalStorage";
import Mutex from "./Mutex";

const BASE_URL = "http://localhost:5137";
const api = axios.create({
  baseURL: BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Centralized token management
function getAuthHeaders(token = null) {
  if (!token) {
    token = getAccessToken();
  }
  return {
    Authorization: `Bearer ${token}`,
  };
}

// Reused error handling function
function handleError(error, errorMessage) {
  console.error(`${errorMessage}:`, error);
  throw error;
}

async function createUser(userData, token = null) {
  try {
    const response = await api.post("/users", userData, {
      headers: {
        ...getAuthHeaders(token),
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error creating user");
  }
}

async function getUserDataByToken(token = null) {
  try {
    const response = await api.get("/users/token", {
      headers: {
        ...getAuthHeaders(token),
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error fetching user data by token");
  }
}

async function getUserDataById(id, token = null) {
  try {
    const response = await api.get(`/users/${id}`, {
      headers: {
        ...getAuthHeaders(token),
        Accept: "application/json",
      },
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error fetching user data by ID");
  }
}

async function deleteUser(userId, token = null) {
  try {
    const response = await api.delete(`/users/${userId}`, {
      headers: getAuthHeaders(token),
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error deleting user");
  }
}

async function fetchPlans() {
  try {
    const response = await api.get("/plans");
    return response.data;
  } catch (error) {
    handleError(error, "Error fetching plans");
  }
}

async function login(email, password) {
  try {
    const requestBody = {
      email,
      password,
    };
    const response = await api.post("/login", requestBody);
    return response.data;
  } catch (error) {
    handleError(error, "Error during login");
  }
}

async function fetchActiveSubscriptionByToken(token = null) {
  try {
    const response = await api.get("/subscriptions/active/token", {
      headers: getAuthHeaders(token),
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error fetching active subscription by token");
  }
}

async function fetchActiveSubscriptionById(id, token = null) {
  try {
    const response = await api.get(`/subscriptions/active/${id}`, {
      headers: getAuthHeaders(token),
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error fetching active subscription by ID");
  }
}

async function createPlan(planData, token = null) {
  try {
    const response = await api.post("/plans", planData, {
      headers: {
        ...getAuthHeaders(token),
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error creating plan");
  }
}

async function deletePlan(planId, token = null) {
  try {
    const response = await api.delete(`/plans/${planId}`, {
      headers: getAuthHeaders(token),
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error deleting plan");
  }
}

async function fetchAllUsersInfo(token = null) {
  try {
    const response = await api.get("/users", {
      headers: getAuthHeaders(token),
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error fetching all users' info");
  }
}

async function fetchUserSubscriptionsByToken(token = null) {
  try {
    const response = await api.get("/subscriptions/token", {
      headers: getAuthHeaders(token),
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error fetching user's subscriptions by token");
  }
}

async function fetchUserSubscriptionsById(id, token = null) {
  try {
    const response = await api.get(`/subscriptions/${id}`, {
      headers: getAuthHeaders(token),
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error fetching user's subscriptions by ID");
  }
}

async function deleteUserSubscriptionById(id, token = null) {
  try {
    const response = await api.delete(`/subscriptions/${id}`, {
      headers: getAuthHeaders(token),
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error deleting user's subscription by ID");
  }
}

async function generateClassDiagramByGithub(
  pat,
  author,
  repoName,
  token = null
) {
  try {
    const requestBody = {
      pat,
      owner: author,
      repoName,
    };
    const response = await api.post("/class-diagram/github", requestBody, {
      headers: {
        ...getAuthHeaders(token),
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error generating class diagram by GitHub");
  }
}

async function generateClassDiagramByFile(file, token = null) {
  try {
    const requestBody = new FormData();
    requestBody.append("file", file);
    const response = await api.post("/class-diagram/zip-file", requestBody, {
      headers: {
        ...getAuthHeaders(token),
        "Content-Type": "multipart/form-data",
      },
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error generating class diagram by file");
  }
}

async function generateFlowChartByCode(code, token = null) {
  try {
    const requestBody = JSON.stringify(code);
    const response = await api.post("/flowchart", requestBody, {
      headers: getAuthHeaders(token),
    });
    return response.data;
  } catch (error) {
    handleError(error, "Error generating flow chart by code");
  }
}

async function refreshToken() {
  try {
    const requestBody = {
      accessToken: getAccessToken(),
      refreshToken: getRefreshToken(),
    };
    const response = await api.post("/refresh-token", requestBody);
    if (response.status === 200) {
      const { accessToken, refreshToken } = response.data;
      setAccessToken(accessToken);
      setRefreshToken(refreshToken);
      return { accessToken, refreshToken };
    } else {
      throw new Error(`HTTP error: ${response.status}`);
    }
  } catch (error) {
    handleError(error, "Error refreshing token");
  }
}

const mutex = new Mutex();

async function makeAuthenticatedRequest(requestFunction, ...args) {
  const release = await mutex.lock();
  try {
    const response = await requestFunction(...args);
    return response;
  } catch (error) {
    if (error.response && error.response.status === 401) {
      console.log("Token expired, attempting refresh...");
      try {
        const tokens = await refreshToken();
        const updatedArgs = args.map((arg) =>
          arg === null ? tokens.accessToken : arg
        );
        const response = await requestFunction(...updatedArgs);
        return response;
      } catch (refreshError) {
        handleError(refreshError, "Error while refreshing token");
      }
    } else {
      handleError(error, "Error during request");
    }
  } finally {
    release();
  }
}

export {
  makeAuthenticatedRequest,
  getUserDataByToken,
  getUserDataById,
  fetchPlans,
  login,
  fetchActiveSubscriptionByToken,
  fetchActiveSubscriptionById,
  createPlan,
  deletePlan,
  fetchAllUsersInfo,
  fetchUserSubscriptionsByToken,
  fetchUserSubscriptionsById,
  deleteUserSubscriptionById,
  generateClassDiagramByGithub,
  generateClassDiagramByFile,
  generateFlowChartByCode,
  refreshToken,
  deleteUser,
  createUser,
};
