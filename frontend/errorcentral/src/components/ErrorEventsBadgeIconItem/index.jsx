import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Badge from '@material-ui/core/Badge';

const useStyles = makeStyles((theme) => ({
  root: {
    '& > *': {
      margin: theme.spacing(2),
    },
  },
}));

const ErrorEventsBadgeIconItem = (props) => {
  const classes = useStyles();
  const defaultProps = {
    color: 'secondary',
    children: props.children,
  };
  return (
    <div className={classes.root}>
      <Badge badgeContent={props.events} max={50} {...defaultProps} />
    </div>
  );
}

export default ErrorEventsBadgeIconItem;