import React from 'react';
import ListItem from '@material-ui/core/ListItem';
import ListItemAvatar from '@material-ui/core/ListItemAvatar';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import ListItemText from '@material-ui/core/ListItemText';
import IconButton from '@material-ui/core/IconButton';
import DeleteIcon from '@material-ui/icons/Delete';
import ArchiveIcon from '@material-ui/icons/Archive';
import UnarchiveIcon from '@material-ui/icons/Unarchive';
import ErrorIconItem from '../ErrorIconItem';

const ErrorItem = () => {
  return (
    <ListItem>
      <ListItemAvatar>
        <ErrorIconItem />
      </ListItemAvatar>
      <ListItemText
        primary="Nome do erro"
        secondary={'Secondary text'}
      />
      <ListItemSecondaryAction>
        <IconButton edge="end" aria-label="unarchive">
          <UnarchiveIcon />
        </IconButton>
        <IconButton edge="end" aria-label="archive">
          <ArchiveIcon />
        </IconButton>
        <IconButton edge="end" aria-label="delete">
          <DeleteIcon />
        </IconButton>
      </ListItemSecondaryAction>
    </ListItem>
  );
};

export default ErrorItem;