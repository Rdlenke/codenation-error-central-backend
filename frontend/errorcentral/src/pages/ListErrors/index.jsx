import React, { useState, useEffect } from 'react';

import api from '../../services/api';

import Container from '@material-ui/core/Container';
import List from '@material-ui/core/List';
import ErrorItem from '../../components/ErrorItem';
import Divider from '@material-ui/core/Divider';
import ListBar from '../../components/ListBar';
import ListNotice from '../../components/ListNotice';

const ListErrors = () => {
  const [errors, setErrors] = useState({ data: null, loading: false, status: 0 });
  useEffect(() => {
    setErrors({ data: null, loading: true, status: 0 });
    api.get('v1/logerrors')
      .then(response => {})
      .catch(error => {
        console.log('error', error);
        setErrors({ loading: false, status: error.request.status });
      });
  }, [])
  return (
    <Container maxWidth="md">
      <ListBar lenght={errors.data ? errors.data.lenght : 0} />
      {!errors.loading ? (
        <List component="nav" aria-label="errors">
          {errors.data && errors.data.lenght > 0 ?
            errors.data.map(error => (
              <div key={error.id}>
                <ErrorItem error={error} />
                <Divider />
              </div>
            )) : <ListNotice status={errors.status} />
          }
        </List>
      ) : (<h1>Carregando...</h1>)}
    </Container>
  );
}

export default ListErrors;