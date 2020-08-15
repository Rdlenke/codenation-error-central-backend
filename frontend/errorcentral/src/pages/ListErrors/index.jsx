import React from 'react';
import Container from '@material-ui/core/Container';
import List from '@material-ui/core/List';
import ErrorItem from '../../components/ErrorItem';

const ListErrors = () => {
  return (
    <Container maxWidth="md">
      <h1>Ola</h1>
      <List>
          <ErrorItem />
          <ErrorItem />
          <ErrorItem />
          <ErrorItem />
          <ErrorItem />
          <ErrorItem />
          <ErrorItem />
          <ErrorItem />
          <ErrorItem />
      </List>
    </Container>
  );
}

export default ListErrors;