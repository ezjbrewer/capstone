import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { login } from "../../managers/authManager";
import { Button, FormFeedback, FormGroup, Input, Label } from "reactstrap";
import "./Auth.css"

export default function Login({ setLoggedInUser }) {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [failedLogin, setFailedLogin] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    login(email, password).then((user) => {
      if (!user) {
        setFailedLogin(true);
      } else {
        setLoggedInUser(user);
        navigate("/");
      }
    });
  };

  return (
    <div className="login-page">
      <div className="login-card">
        <h3>Login</h3>
        <FormGroup>
          <Label>Email</Label>
          <Input
            invalid={failedLogin}
            type="text"
            value={email}
            onChange={(e) => {
              setFailedLogin(false);
              setEmail(e.target.value);
            }}
          />
        </FormGroup>
        <FormGroup>
          <Label>Password</Label>
          <Input
            invalid={failedLogin}
            type="password"
            value={password}
            onChange={(e) => {
              setFailedLogin(false);
              setPassword(e.target.value);
            }}
          />
          <FormFeedback>Login failed.</FormFeedback>
        </FormGroup>
      </div>
      <div className="login-btn-card">
        <Button className="login-btn" onClick={handleSubmit}>
          Login
        </Button>
        <p>
          Not signed up? Register <Link to="/register">here</Link>
        </p>
      </div>
    </div>
  );
}
