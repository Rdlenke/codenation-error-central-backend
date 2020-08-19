import React from 'react';

import api from '../../services/api';

import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import IconButton from '@material-ui/core/IconButton';
import DeleteIcon from '@material-ui/icons/Delete';
import ArchiveIcon from '@material-ui/icons/Archive';
import UnarchiveIcon from '@material-ui/icons/Unarchive';
import Tooltip from '@material-ui/core/Tooltip';

const ErrorActionsItem = (props) => {
  function handleArchiveClick(event) {
    event.preventDefault();
    event.stopPropagation();
    api.patch(`v1/logerros/archive/${props.error.id}`)
      .then(response => {})
      .catch(error => {});
  }
  
  function handleUnarchiveClick(event) {
    event.preventDefault();
    event.stopPropagation();
    api.patch(`v1/logerros/unarchive/${props.error.id}`)
      .then(response => {})
      .catch(error => {});
  }

  function handleDeleteClick(event) {
    event.preventDefault();
    event.stopPropagation();
    api.delete(`v1/logerros/${props.error.id}`)
      .then(response => {})
      .catch(error => {});
  }

  function getButtonArchiving(filed) {
    if (filed) {
      return (
        <Tooltip title="Desarquivar">
          <IconButton edge="end" aria-label="unarchive" onClick={handleUnarchiveClick}>
            <UnarchiveIcon />
          </IconButton>
        </Tooltip>
      );
    } else {
      return (
        <Tooltip title="Arquivar">
          <IconButton edge="end" aria-label="archive" onClick={handleArchiveClick}>
            <ArchiveIcon />
          </IconButton>
        </Tooltip>
      );
    }
  }

  return (
    <ListItemSecondaryAction>
      {getButtonArchiving(props.error.filed)}
      <Tooltip title="Remover">
        <IconButton edge="end" aria-label="delete" onClick={handleDeleteClick}>
          <DeleteIcon />
        </IconButton>
      </Tooltip>
    </ListItemSecondaryAction>
  );
}

export default ErrorActionsItem;