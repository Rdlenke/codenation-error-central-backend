import React from 'react';
import Container from '@material-ui/core/Container';
import List from '@material-ui/core/List';
import ErrorItem from '../../components/ErrorItem';
import Divider from '@material-ui/core/Divider';
import SnackbarAlert from '../../components/SnackbarAlert';

const ListErrors = () => {
  return (
    <Container maxWidth="md">
      <h1>Ola</h1>
      <SnackbarAlert />
      <List component="nav" aria-label="errors">
        <ErrorItem />
        <Divider />
        <ErrorItem />
        <Divider />
        <ErrorItem />
        <Divider />
        <ErrorItem />
        <Divider />
        <ErrorItem />
        <Divider />
        <ErrorItem />
        <Divider />
        <ErrorItem />
        <Divider />
        <ErrorItem />
        <Divider />
        <ErrorItem />
        <Divider />
      </List>
    </Container>
  );
}

export default ListErrors;