import React from 'react';
import { Route, BrowserRouter } from 'react-router-dom';
import Signin from './pages/Signin';
import Signup from './pages/Signup';

const Routes = () => {
  return (
    <BrowserRouter>
      <Route component={Signin} path='/' exact />
      <Route component={Signup} path='/join' />
    </BrowserRouter>
  );
};

export default Routes;
