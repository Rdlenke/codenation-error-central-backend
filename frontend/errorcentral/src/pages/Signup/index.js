import React, { useState } from "react";
import { Scope } from "@unform/core";
import api from '../../services/api';
import { Form } from "@unform/web";
import Input from "../../components/Form/input";
// import "../../components/Form/styles.css";
import logoImg from '../../assets/images/ErrorCentralLogo.png';
import './styles.css'



const Signup = () => {
  const [state, setState] = useState({ data: [], loading: false, status: 0 });

  function handleSubmit(data, { reset }) {
    setState({data: [], loading: true, status: 0})

    api.post('v1/User/CreateUser', data)
      .then(response => {
        setState({ loading: false})

        console.log(response.data);
      })
      .catch(error => {
        setState({ data: error, loading: false, status: 0 })
      })

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
          <Input name="FirstName" label="Nome" />
          <Input name="LastName" label="Sobrenome" />
          <Input name="Email" label="E-mail" type="email" />
          <Input name="Password" label="Senha" type="password" />

          <button type="submit">Cadastrar</button>
        </div>
      <a href="/login">Login</a>
    </Form>
  );
};

export default Signup;