import React from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemAvatar from '@material-ui/core/ListItemAvatar';
import ListItemText from '@material-ui/core/ListItemText';
import ErrorIconItem from '../ErrorIconItem';
import ErrorActionsItem from '../ErrorActionsItem';

const ErrorItem = () => {


  return (
    <ListItem button>
      <ListItemAvatar>
        <ErrorIconItem />
      </ListItemAvatar>
      <ListItemText
        primary="Nome do erro"
        secondary={'Secondary text'}
      />
      <ErrorActionsItem />
    </ListItem>
  );
};

export default ErrorItem;