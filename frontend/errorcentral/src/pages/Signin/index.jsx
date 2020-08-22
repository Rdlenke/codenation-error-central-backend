import React, { useState } from "react";
import { Scope } from "@unform/core";
import api from '../../services/api';

import { Form } from "@unform/web";
// import Input from "../../components/Form/input";
// import "../../components/Form/styles.css";
import logoImg from '../../assets/images/ErrorCentralLogo.png';

import { connect } from 'react-redux';
import { addUser } from '../../redux/actions/actionCreators';
import { useHistory } from "react-router-dom";

import clsx from 'clsx';
import { makeStyles } from '@material-ui/core/styles';
import IconButton from '@material-ui/core/IconButton';
import Input from '@material-ui/core/Input';
import FilledInput from '@material-ui/core/FilledInput';
import OutlinedInput from '@material-ui/core/OutlinedInput';
import InputLabel from '@material-ui/core/InputLabel';
import InputAdornment from '@material-ui/core/InputAdornment';
import FormHelperText from '@material-ui/core/FormHelperText';
import FormControl from '@material-ui/core/FormControl';
import TextField from '@material-ui/core/TextField';
import Visibility from '@material-ui/icons/Visibility';
import VisibilityOff from '@material-ui/icons/VisibilityOff';


const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    flexWrap: 'wrap',
  },
  margin: {
    margin: theme.spacing(1),
  },
  withoutLabel: {
    marginTop: theme.spacing(3),
  },
  textField: {
    width: '25ch',
  },
}));

const Signin = (props) => {
  const classes = useStyles();
  const [state, setState] = useState({ data: [], loading: false, status: 0 });
  const [values, setValues] = useState({
    email: '',
    password: '',
    showPassword: false,
  });
  const history = useHistory();

  const handleChange = (prop) => (event) => {
    setValues({ ...values, [prop]: event.target.value });
  };

  const handleClickShowPassword = () => {
    setValues({ ...values, showPassword: !values.showPassword });
  };

  const handleMouseDownPassword = (event) => {
    event.preventDefault();
  };

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
          {/* <Input name="Email" label="E-mail" type="email" /> */}
          <TextField
            id="email"
            label="E-mail"
            style={{ margin: 8 }}
            placeholder="email@email.com"
            type="email"
            fullWidth
            margin="normal"
            InputLabelProps={{
              shrink: true,
            }}
            variant="outlined"
          />
          <FormControl className={clsx(classes.margin, classes.textField)} variant="outlined">
            <InputLabel htmlFor="outlined-adornment-password">Password</InputLabel>
            <OutlinedInput
              id="outlined-adornment-password"
              type={values.showPassword ? 'text' : 'password'}
              value={values.password}
              onChange={handleChange('password')}
              endAdornment={
                <InputAdornment position="end">
                  <IconButton
                    aria-label="toggle password visibility"
                    onClick={handleClickShowPassword}
                    onMouseDown={handleMouseDownPassword}
                    edge="end"
                  >
                    {values.showPassword ? <Visibility /> : <VisibilityOff />}
                  </IconButton>
                </InputAdornment>
              }
              labelWidth={70}
            />
          </FormControl>
          <button type="submit">Entrar</button>
        </div>
        <a href="/join">Registre-se</a>
      </Form>
    </div>
  );
};

const mapDispatchToProps = { addUser }

export default connect(null, mapDispatchToProps)(Signin);
