import React from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemAvatar from '@material-ui/core/ListItemAvatar';
import ListItemText from '@material-ui/core/ListItemText';
import ErrorIconItem from '../ErrorIconItem';
import ErrorActionsItem from '../ErrorActionsItem';
import { Link } from 'react-router-dom';

const ErrorItem = (props) => {
  return (
    <ListItem button component={Link} to={`erros/${1}`}>
      <ListItemAvatar>
        <ErrorIconItem />
      </ListItemAvatar>
      <ListItemText
        primary={props.title}
        secondary={props.descripton}
      />
      <ErrorActionsItem />
    </ListItem>
  );
};

export default ErrorItem;