import React, { useState } from "react";
import { Scope } from "@unform/core";
import api from '../../services/api';

import { Form } from "@unform/web";
import Input from "../../components/Form/input";
// import "../../components/Form/styles.css";
import logoImg from '../../assets/images/ErrorCentralLogo.png';

import { connect } from 'react-redux';
import { addUser } from '../../redux/actions/actionCreators';
import { useHistory } from "react-router-dom";


const Signin = (props) => {
  const [state, setState] = useState({ data: [], loading: false, status: 0 });
  const history = useHistory();

  function handleSubmit(data, { reset }) {
    setState({data: [], loading: true, status: 0})

    // props.addUser({email: "email", firstName: "name", lastName: "name", token: "token", guid: "guid"})
    // history.push("/");

    api.post('v1/User/AuthenticateUser', data)
      .then(response => {

        setState({ loading: false });

        props.addUser(response.data);

        history.push("/");

      })
      .catch(error => {
        setState({ data: error, loading: false, status: 0 })
      });

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
          <Input name="Email" label="E-mail" type="email" />
          <Input name="Password" label="Senha" type="password" />

          <button type="submit">Entrar</button>
        </div>
        <a href="/join">Registre-se</a>
      </Form>
    </div>
  );
};

const mapDispatchToProps = { addUser }

export default connect(null, mapDispatchToProps)(Signin);
