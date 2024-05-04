const Roles = {
  ADMINISTRATOR: 1,
  USER: 2,
  GUEST: 3,
};

const getRoleStr = (roleId) => {
  switch (roleId) {
    case Roles.ADMINISTRATOR:
      return "Администратор";
    case Roles.USER:
      return "Пользователь";
    case Roles.GUEST:
      return "Гость";
    default:
      return "Неизвестный";
  }
};

export { getRoleStr, Roles };
