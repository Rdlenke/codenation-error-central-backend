import React, { useState } from "react";
import api from '../../services/api';
import { Form } from "@unform/web";
import Input from "../../components/Form/input";
import logoImg from '../../assets/images/ErrorCentralLogo.png';
import './styles.css'
import { connect } from 'react-redux';
import { addUser } from '../../redux/actions/actionCreators';
import { useHistory } from "react-router-dom";


const Signup = (props) => {
  const [state, setState] = useState({ data: [], loading: false, status: 0 });
  const history = useHistory();

  function handleSubmit(data, { reset }) {
    setState({data: [], loading: true, status: 0})

    // props.addUser({email: "email", firstName: "name", lastName: "name", token: "token", guid: "guid"})
    // history.push("/");

    api.post('v1/User/CreateUser', data)
      .then(response => {

        setState({ loading: false});

        props.addUser(response.data);

        history.push("/");

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

const mapDispatchToProps = { addUser };


export default connect(null, mapDispatchToProps)(Signup);