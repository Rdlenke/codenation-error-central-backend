import React, { useState, useEffect } from 'react';

import api from '../../services/api';

import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import Divider from '@material-ui/core/Divider';
import CircularProgress from '@material-ui/core/CircularProgress';
import List from '@material-ui/core/List';
import Grid from '@material-ui/core/Grid';

import ErrorItem from '../../components/ErrorItem';
import ListBar from '../../components/ListBar';
import ListNotice from '../../components/ListNotice';

import Errors from '../../mock/errors';



const useStyles = makeStyles({
  containerProgress: {
    display: 'flex',
    flexDirection: 'column',
    width: '100%',
    minHeight: '80vh',
    height: '100%',
    justifyContent: 'center',
    alignItems: 'center',
  },
});

const ListErrors = () => {
  const classes = useStyles();
  const [errors, setErrors] = useState({ data: [], loading: false, status: 0 });
  useEffect(() => {
    setErrors({ data: [], loading: true, status: 0 });
    api.get('v1/logerrors')
      .then(response => {
        setErrors({ data: response.data, loading: false, status: response.status });
      }).catch(error => {
        // setErrors({ data: null, loading: false, status: error.request.status });
        setTimeout(function() {
          setErrors({ data: Errors, loading: false, status: 0 });
        }, 1000);
      });
  }, []);

  return (
    <Container maxWidth="xl">
      <ListBar lenght={errors.data ? errors.data.length : 0} />
      <Grid container spacing={2}>
        <Grid item>
          <h1>Ola</h1>
        </Grid>
        <Grid
          item xs={12} sm container
          direction="column"
        >
          {!errors.loading ? (
            <List component="nav" aria-label="errors">
              {errors.data && errors.data.length > 0 ?
                errors.data.map(error => (
                  <div key={error.id}>
                    <ErrorItem error={error} />
                    <Divider />
                  </div>
                )) : <ListNotice status={errors.status} />
              }
            </List>
          ) : (
            <div className={classes.containerProgress}>
              <CircularProgress />
            </div>
          )}
        </Grid>
      </Grid>
    </Container>
  );
}

export default ListErrors;