const getUserInfo = () => {
  const userInfo = localStorage.getItem("user_info");
  return userInfo ? JSON.parse(userInfo) : null;
};

const setUserInfo = (user) => {
  return localStorage.setItem("user_info", JSON.stringify(user));
};

const getRefreshToken = () => {
  return localStorage.getItem("refresh_token");
};

const setRefreshToken = (token) => {
  return localStorage.setItem("refresh_token", token);
};

const getAccessToken = () => {
  return localStorage.getItem("access_token");
};

const setAccessToken = (token) => {
  return localStorage.setItem("access_token", token);
};

export {
  getUserInfo,
  setUserInfo,
  getAccessToken,
  setAccessToken,
  getRefreshToken,
  setRefreshToken,
};
