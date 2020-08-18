import React from "react";
import { Scope } from "@unform/core";
import { Form } from "@unform/web";
import Input from "../../components/Form/input";
// import "../../components/Form/styles.css";
import logoImg from '../../assets/images/ErrorCentralLogo.png';

const Signin = () => {
  function handleSubmit(data, { reset }) {
    console.log(data);

    reset();
  }

  return (
    <div className="whiteBackground">
      <Form onSubmit={handleSubmit}>
        <img
          src={logoImg}
          // height="150"
          // width="175"
          alt="logo"
        />

        <div className="form-container">
          <Input name="email" label="E-mail" type="email" />
          <Input name="password" label="Senha" type="password" />

          <button type="submit">Entrar</button>
        </div>
        <a href="/join">Registre-se</a>
      </Form>
    </div>
  );
};

export default Signin;
