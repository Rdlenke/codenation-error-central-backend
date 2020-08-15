import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import ListFilterMenu from '../ListFilterMenu';


const useStyles = makeStyles({
  root: {
    minWidth: 275,
  },
  content: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItens: 'center',
    padding: 0,
    '&:last-child': {
      padding: 0,
    },
  },
  title: {
    fontSize: 14,
    alignSelf: 'center',
    paddingLeft: 5
  },
});

const ListBar = () => {
  const classes = useStyles();

  return (
    <Card className={classes.root}>
      <CardContent className={classes.content}>
        <Typography className={classes.title} color="textSecondary">
          120 resultados
        </Typography>
        <ListFilterMenu />
      </CardContent>
    </Card>
  );
}

export default ListBar;