import React, { useState } from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemAvatar from '@material-ui/core/ListItemAvatar';
import ListItemText from '@material-ui/core/ListItemText';
import ErrorIconItem from '../ErrorIconItem';
import ErrorActionsItem from '../ErrorActionsItem';
import { Link } from 'react-router-dom';

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
    <ListItem button component={Link} to={`erros/${props.error.id}`} onPointerEnter={handleItemPointerEnter} onPointerLeave={handleItemPointerLeave}>
      <ListItemAvatar>
        <ErrorIconItem />
      </ListItemAvatar>
      <ListItemText
        primary={props.error.title}
        secondary={props.error.origin}
      />
      {showActions && <ErrorActionsItem error={props.error} />}
    </ListItem>
  );
};

export default ErrorItem;