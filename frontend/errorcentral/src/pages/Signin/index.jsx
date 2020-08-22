import React, { useState } from "react";
import api from '../../services/api';
import logoImg from '../../assets/images/ErrorCentralLogo.png';

import { connect } from 'react-redux';
import { addUser } from '../../redux/actions/actionCreators';
import { useHistory } from "react-router-dom";

import { makeStyles } from '@material-ui/core/styles';
import IconButton from '@material-ui/core/IconButton';
import OutlinedInput from '@material-ui/core/OutlinedInput';
import InputLabel from '@material-ui/core/InputLabel';
import InputAdornment from '@material-ui/core/InputAdornment';
import FormControl from '@material-ui/core/FormControl';
import TextField from '@material-ui/core/TextField';
import Visibility from '@material-ui/icons/Visibility';
import VisibilityOff from '@material-ui/icons/VisibilityOff';
import Button from '@material-ui/core/Button';
import Paper from '@material-ui/core/Paper';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    minHeight: '90vh',
    flexWrap: 'wrap',
    justifyContent: 'center',
    alignItems: 'center',
    alignSelf: 'center',
    flexDirection: 'column'
  },
  marginBottom: {
    marginBottom: theme.spacing(2),
  },
  form: {
    display: 'flex',
    width: '300px',
    flexDirection: 'column',
    padding: '20px 40px',
  },
  image: {
    margin: '0 auto',
    height: '150px',
    minWidth: '100px',
    maxWidth: '100%',
  }
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
    <form className={classes.root} onSubmit={handleSubmit}>
      <Paper className={classes.form}>
        <img
          src={logoImg}
          alt="logo"
          className={classes.image}
        />

        <TextField
          id="email"
          name="email"
          label="E-mail"
          placeholder="email@email.com"
          type="email"
          fullWidth
          margin="normal"
          variant="outlined"
          className={classes.marginBottom}
        />
        <FormControl fullWidth variant="outlined">
          <InputLabel htmlFor="password">Senha</InputLabel>
          <OutlinedInput
            id="password"
            name="password"
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
            className={classes.marginBottom}
          />
        </FormControl>
        <Button
          variant="contained"
          color="primary"
          type="submit"
          className={classes.marginBottom}
        >
          Entrar
        </Button>
        <a href="/join" className={classes.marginBottom}>Registre-se</a>
      </Paper>
    </form>
  );
};

const mapDispatchToProps = { addUser }

export default connect(null, mapDispatchToProps)(Signin);
