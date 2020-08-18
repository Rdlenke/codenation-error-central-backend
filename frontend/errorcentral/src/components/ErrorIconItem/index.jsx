import React from 'react';

import { makeStyles } from '@material-ui/core/styles';
import { yellow, red, lightBlue } from '@material-ui/core/colors';
import Avatar from '@material-ui/core/Avatar';
import WarningIcon from '@material-ui/icons/Warning';
import ErrorIcon from '@material-ui/icons/Error';
import BugReportIcon from '@material-ui/icons/BugReport';

import ErrorEventsBadgeIconItem from '../ErrorEventsBadgeIconItem';

export const useStyles = makeStyles((theme) => ({
  red: {
    color: theme.palette.getContrastText(red[500]),
    backgroundColor: red[500],
  },
  yellow: {
    color: theme.palette.getContrastText(yellow[600]),
    backgroundColor: yellow[600],
  },
  lightBlue: {
    color: theme.palette.getContrastText(lightBlue[500]),
    backgroundColor: lightBlue[500],
  },
}));

const ErrorIconItem = (props) => {
  const classes = useStyles();

  function getColorAvatarErrorLevel(level) {
    return {
      1: classes.lightBlue,
      2: classes.yellow,
      3: classes.red
    }[level];
  }

  function getIconErrorLevel(level) {
    return {
      1: <BugReportIcon />,
      2: <WarningIcon />,
      3: <ErrorIcon />
    }[level];
  }

  return (
    <>
      {props.level && (
        <ErrorEventsBadgeIconItem events={props.events}>
          <Avatar variant="rounded" className={getColorAvatarErrorLevel(props.level)}>
            {getIconErrorLevel(props.level)}
          </Avatar>
        </ErrorEventsBadgeIconItem>
      )}
    </>
  );
}

export default ErrorIconItem;