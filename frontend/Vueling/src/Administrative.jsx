import { Form } from "./Form.jsx";
import { useState } from "react";

export function Administrative() {
  const [auth, setAut] = useState(false);
  const [usernameLogin, setUserNameLogin] = useState("");
  const [passwordLogin, setPasswordLogin] = useState("");
  const handleUserNameLogin = (e) => setUserNameLogin(e.target.value);
  const handlePasswordLogin = (e) => setPasswordLogin(e.target.value);

  const [formData, setFormData] = useState({
    username: "",
    email: "",
    password: "",
    role: "",
  });

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFormData((prevFormData) => ({
      ...prevFormData,
      [name]: value,
    }));
  };

  async function loginUser() {
    try {
      const response = await fetch("http://localhost:8001/api/Auth/login", {
        method: "post",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          username: usernameLogin,
          password: passwordLogin,
        }),
      });
      const responseAuth = await response.json();
      if (responseAuth.message === "Success") {
        setAut(true);
      } else {
        alert("Login failed", responseAuth.message);
      }
    } catch (err) {
      console.log(err);
    }
  }
  async function signupUser() {
    try {
      const response = await fetch("http://localhost:8001/api/Auth/SignUp", {
        method: "POST",
        body: JSON.stringify({
          userName: username,
          password: password,
          email: email,
          roles: [role],
        }),
        headers: {
          "Content-Type": "application/json",
        },
      });
      const responseAuth = await response.json();
      if (responseAuth.message === "User Registration Successful") {
        alert("User registered");
      } else {
        alert("User not registered", responseAuth.message);
      }
    } catch (error) {
      console.log(error);
    }
  }

  const handleSubmitSignup = async (event) => {
    event.preventDefault();
    signupUser();
  };

  const handleSubmitLogin = (event) => {
    event.preventDefault();
    loginUser();
  };

  return (
    <section className="component-admin">
      {auth ? (
        <>
          <Form />
          <button
            className="control-select btn-logout"
            onClick={() => {
              setAut(false);
            }}
          >
            Logout
          </button>
        </>
      ) : (
        <section className="fake-section">
         <p>Dear user, login to query data.</p>
        </section>
      )}
      {!auth && (
        <section className="forms-login-register">
          <form className="form-login" onSubmit={handleSubmitLogin}>
            <p className="control-label">Login user</p>
              <label className="control-label" htmlFor="username">
                Username:
              <input
                className="control-select"
                type="text"
                id="username"
                value={usernameLogin}
                onChange={handleUserNameLogin}
                required
              />
             </label>
              <label className="control-label" htmlFor="password">
                Password:
              <input
                className="control-select"
                type="password"
                id="password"
                value={passwordLogin}
                onChange={handlePasswordLogin}
                required
              />
              </label>
            <button type="submit" className="control-select">
              Log In
            </button>
          </form>
          <form className="form-register" onSubmit={handleSubmitSignup}>
            <p className="control-label">Register new user</p>
            <label className="control-label">
              Username:
              <input
                className="control-select"
                type="text"
                name="username"
                value={formData.username}
                onChange={handleInputChange}
              />
            </label>
            <label className="control-label">
              Email:
              <input
                className="control-select"
                type="email"
                name="email"
                value={formData.email}
                onChange={handleInputChange}
              />
            </label>
            <label className="control-label">
              Password:
              <input
                className="control-select"
                type="password"
                name="password"
                value={formData.password}
                onChange={handleInputChange}
              />
            </label>
            <label className="control-label">
              Role:
              <input
                className="control-select"
                type="text"
                name="role"
                value={formData.role}
                onChange={handleInputChange}
              />
            </label>
            <button type="submit" className="control-select">
              Register
            </button>
          </form>
        </section>
      )}
    </section>
  );
}
