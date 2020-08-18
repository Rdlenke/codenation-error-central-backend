import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { lightBlue } from '@material-ui/core/colors';
import Avatar from '@material-ui/core/Avatar';
import NotListedLocationOutlinedIcon from '@material-ui/icons/NotListedLocationOutlined';
import Typography from '@material-ui/core/Typography';

export const useStyles = makeStyles((theme) => ({
  container: {
    display: 'flex',
    flexDirection: 'column',
    width: '100%',
    minHeight: '80vh',
    height: '100%',
    justifyContent: 'center',
    alignItems: 'center',
  },
  avatar: {
    width: 200,
    height: 200,
    marginBottom: 40
  },
  icon: {
    fontSize: 130,
  },
  lightBlue: {
    color: theme.palette.getContrastText(lightBlue[100]),
    backgroundColor: lightBlue[100],
  },
  textCenter: {
    textAlign: 'center',
  }
}));

const ListNotice = (props) => {
  const classes = useStyles();

  function getColorAvatarListNotice(status) {
    return {
      404: classes.lightBlue
    }[status];
  }

  function getIconListNotice(status) {
    return {
      404: <NotListedLocationOutlinedIcon className={classes.icon} />
    }[status];
  }
  
  function getTitleListNotice(status) {
    return {
      404: 'Não encontramos resultado para a sua pesquisa'
    }[status];
  }
  
  function getSubTitleListNotice(status) {
    return {
      404: 'Verifique se a palavra está escrita corretamente.'
    }[status];
  }

  return (
    <div className={classes.container}>
      {props.status !== 200 && (
        <>
          <Avatar className={[getColorAvatarListNotice(props.status), classes.avatar].join(' ')}>
            {getIconListNotice(props.status)}
          </Avatar>
          <Typography variant="h6" component="h2" color="textSecondary" className={classes.textCenter}>
            {getTitleListNotice(props.status)}
          </Typography>
          <Typography color="textSecondary" className={classes.textCenter}>
            {getSubTitleListNotice(props.status)}
          </Typography>
        </>
      )}
    </div>
  );
}

export default ListNotice;