import React, { useState } from 'react';
import { Link } from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import AccountCircle from '@material-ui/icons/AccountCircle';
import MenuItem from '@material-ui/core/MenuItem';
import Menu from '@material-ui/core/Menu';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import { useHistory } from "react-router-dom";
import { removeUser } from '../../redux/actions/actionCreators';


const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  },
  actions: {
    '& > *': {
      margin: theme.spacing(1),
      textTransform: 'none',
    },
  },
}));

const Header = (props) => {
  const classes = useStyles();
  const auth = props.user.token !== "" ? true : false;

  const [anchorEl, setAnchorEl] = useState(null);
  const open = Boolean(anchorEl);

  const history = useHistory();

  const handleMenu = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleClick = () => {
    props.removeUser();
    history.push('/login');
  }

  return (
    <div className={classes.root}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" className={classes.title}>
            Central de Erros
          </Typography>
          {auth ? (
            <div>
              <Button
                aria-label="account of current user"
                aria-controls="menu-appbar"
                aria-haspopup="true"
                variant="contained"
                color="primary"
                size="large"
                startIcon={<AccountCircle fontSize="large" />}
                onClick={handleMenu}
                disableElevation
              >
                {`${props.user.firstName} ${props.user.lastName}`}
              </Button>
              <Menu
                id="menu-appbar"
                anchorEl={anchorEl}
                anchorOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                keepMounted
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                open={open}
                onClose={handleClose}
              >
                <MenuItem onClick={handleClick}>Sair</MenuItem>
              </Menu>
            </div>
          ) : (
            <div className={classes.actions}>
              <Button
                component={Link}
                to="/login"
                variant="contained"
                color="primary"
                disableElevation
              >
                Sign in
              </Button>
              <Button
                component={Link}
                to="/join"
                variant="outlined"
                color="inherit"
                disableElevation
              >
                Sign up
              </Button>
            </div>
          )}
        </Toolbar>
      </AppBar>
    </div>
  );
}

const mapStateToProps = state => {
  return {
    user: state.user
  };
}

const mapDispatchToProps = { removeUser };

export default connect(mapStateToProps, mapDispatchToProps)(Header);