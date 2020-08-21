import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { lightBlue } from '@material-ui/core/colors';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import Popover from '@material-ui/core/Popover';
import Button from '@material-ui/core/Button';

import FilterErrorOrder from '../FilterErrorOrder';
import FilterErrorEnvironment from '../FilterErrorEnvironment';
import { Divider } from '@material-ui/core';

const useStyles = makeStyles({
  button: {
    textTransform: 'none',
    color: lightBlue[500],
  },
  iconButton: {
    paddingLeft: 3,
  },
  popover: {
    display: 'flex',
    flexDirection: 'column',
  }
});

const ListFilterMenu = () => {
  const classes = useStyles();
  const [anchorEl, setAnchorEl] = useState(null);

  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const open = Boolean(anchorEl);
  const id = open ? 'simple-popover' : undefined;

  return (
    <div>
      <Button
        aria-describedby={id}
        className={classes.button}
        onClick={handleClick}
      >
        Filtrar
        <ExpandMoreIcon fontSize="small" className={classes.iconButton} />
      </Button>
      <Popover
        id={id}
        open={open}
        anchorEl={anchorEl}
        onClose={handleClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'center',
        }}
      >
        <div className={classes.popover}>
          <FilterErrorOrder />
          <Divider />
          <FilterErrorEnvironment />
        </div>
      </Popover>
    </div>
  );
}

export default ListFilterMenu;