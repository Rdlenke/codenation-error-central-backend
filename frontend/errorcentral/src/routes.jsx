import React from 'react';
import { Route, BrowserRouter } from 'react-router-dom';
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
      <ProtectedRoute component={ListErrors} path='/' exact />
      <ProtectedRoute component={DetailsError} path='/erros/:id' />
      <Route component={Signin} path='/login' />
      <Route component={Signup} path='/join' />
    </BrowserRouter>
  );
};

export default Routes;
