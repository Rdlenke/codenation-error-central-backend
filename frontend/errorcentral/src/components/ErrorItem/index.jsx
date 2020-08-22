import React, { useState } from 'react';

import ListItem from '@material-ui/core/ListItem';
import ListItemAvatar from '@material-ui/core/ListItemAvatar';
import ListItemText from '@material-ui/core/ListItemText';
import Typography from '@material-ui/core/Typography';

import ErrorIconItem from '../ErrorIconItem';
import ErrorActionsItem from '../ErrorActionsItem';

import { Link } from 'react-router-dom';
import * as moment from 'moment';
import 'moment/locale/pt-br';
moment.locale('pt-br');

const ErrorItem = (props) => {
  const [showActions, setShowActions] = useState(false);
  function handleItemPointerEnter(event) {
    event.preventDefault();
    event.stopPropagation();
    setShowActions(true);
  }
  function handleItemPointerLeave(event) {
    event.preventDefault();
    event.stopPropagation();
    setShowActions(false);
  }
  return (
    <ListItem button component={Link} to={ `erros/${props.error.id}`} onPointerEnter={handleItemPointerEnter} onPointerLeave={handleItemPointerLeave}>
      <ListItemAvatar>
        <ErrorIconItem level={props.error.level} events={props.error.events} />
      </ListItemAvatar>
      <ListItemText
        primary={props.error.title}
        secondary={props.error.source}
      />
      {showActions ? (
        <ErrorActionsItem error={props.error} />
      ) : (
        <Typography color="textSecondary">
          {moment(props.error.date).fromNow()}
        </Typography>
      )}
    </ListItem>
  );
};

export default ErrorItem;