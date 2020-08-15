import React from 'react';
import { Route, BrowserRouter } from 'react-router-dom';
import Signin from './pages/Signin';
import Signup from './pages/Signup';
import ListErrors from './pages/ListErrors';

const Routes = () => {
  return (
    <BrowserRouter>
      <Route component={ListErrors} path='/' exact />
      <Route component={Signin} path='/login' exact />
      <Route component={Signup} path='/join' />
    </BrowserRouter>
  );
};

export default Routes;
