const UserService = (() => {
  let user = window.currentUser || null;

  return {
    getUser() {
      return user;
    },
    getUserId() {
      return user ? user.id : null;
    },
    getUserName() {
      return user ? user.name : null;
    },
    setUser(newUser) {
      user = newUser;
      window.currentUser = newUser;
    },
    clearUser() {
      user = null;
      window.currentUser = null;
    }
  };
})();
