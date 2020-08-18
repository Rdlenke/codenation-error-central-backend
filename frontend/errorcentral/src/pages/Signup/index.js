import React from "react";
import { Scope } from "@unform/core";
import { Form } from "@unform/web";
import Input from "../../components/Form/input";
// import "../../components/Form/styles.css";
import logoImg from '../../assets/images/ErrorCentralLogo.png';
import './styles.css'

const Signup = () => {
  function handleSubmit(data, { reset }) {
    console.log(data);

    reset();
  }

  return (
    <Form onSubmit={handleSubmit}>
      <img
        src={logoImg}
        // height="150"
        // width="175"
        alt="logo"
      />
        <div className="form-container">
          <Input name="name" label="Nome" />
          <Input name="name" label="Sobrenome" />
          <Input name="email" label="E-mail" type="email" />
          <Input name="password" label="Senha" type="password" />

          <button type="submit">Cadastrar</button>
        </div>
      <a href="/login">Login</a>
    </Form>
  );
};

export default Signup;