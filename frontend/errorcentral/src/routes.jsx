import React from 'react';
import { Route, BrowserRouter, Switch } from 'react-router-dom';
import Signin from './pages/Signin';
import Signup from './pages/Signup';
import ListErrors from './pages/ListErrors';
import DetailsError from './pages/DetailsError';
import Header from './components/Header';
import ProtectedRoute from './components/ProtectedRoute'

const Routes = () => {
  return (
    <BrowserRouter>
      <Header />
      <Switch>

        <ProtectedRoute component={DetailsError} path='/erros/:id' exact/>
        <ProtectedRoute component={ListErrors} path='/' exact />
        <Route component={Signin} path='/login' />
        <Route component={Signup} path='/join' />
      </Switch>
    </BrowserRouter>
  );
};

export default Routes;
