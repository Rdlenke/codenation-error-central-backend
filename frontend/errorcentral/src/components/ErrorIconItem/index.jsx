import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { yellow, red, lightBlue } from '@material-ui/core/colors';
import Avatar from '@material-ui/core/Avatar';
import WarningIcon from '@material-ui/icons/Warning';
import ErrorIcon from '@material-ui/icons/Error';
import BugReportIcon from '@material-ui/icons/BugReport';

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
    if (level === 1)
      return classes.lightBlue;
    else if (level === 2)
      return classes.yellow;
    else
      return classes.red;
  }

  function getIconErrorLevel(level) {
    if (level === 1)
      return <BugReportIcon />;
    else if (level === 2)
      return <WarningIcon />;
    else
      return <ErrorIcon />;
  }

  return (
    <Avatar variant="rounded" className={getColorAvatarErrorLevel(props.level)}>
      {getIconErrorLevel(props.level)}
    </Avatar>
  );
}

export default ErrorIconItem;