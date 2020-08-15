import React from 'react';

import api from '../../services/api';

import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import IconButton from '@material-ui/core/IconButton';
import DeleteIcon from '@material-ui/icons/Delete';
import ArchiveIcon from '@material-ui/icons/Archive';
import UnarchiveIcon from '@material-ui/icons/Unarchive';

const ErrorActionsItem = (props) => {
  function handleArchiveClick(event) {
    event.preventDefault();
    event.stopPropagation();
    api.patch(`v1/logerros/archive/${props.id}`)
      .then(response => {})
      .catch(error => {});
  }
  
  function handleUnarchiveClick(event) {
    event.preventDefault();
    event.stopPropagation();
    api.patch(`v1/logerros/unarchive/${props.id}`)
      .then(response => {})
      .catch(error => {});
  }
  
  function handleDeleteClick(event) {
    event.preventDefault();
    event.stopPropagation();
    api.delete(`v1/logerros/${props.id}`)
      .then(response => {})
      .catch(error => {});
  }

  function getButtonArchiving(filed) {
    if (filed) {
      return (
        <IconButton edge="end" aria-label="unarchive" onClick={handleUnarchiveClick}>
          <UnarchiveIcon />
        </IconButton>
      );
    } else {
      return (
        <IconButton edge="end" aria-label="archive" onClick={handleArchiveClick}>
          <ArchiveIcon />
        </IconButton>
      );
    }
  }

  return (
    <ListItemSecondaryAction>
      {getButtonArchiving(props.filed)}
      <IconButton edge="end" aria-label="delete" onClick={handleDeleteClick}>
        <DeleteIcon />
      </IconButton>
    </ListItemSecondaryAction>
  );
}

export default ErrorActionsItem;