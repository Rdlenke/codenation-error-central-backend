import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormControl from '@material-ui/core/FormControl';
import FormLabel from '@material-ui/core/FormLabel';

import Accordion from '@material-ui/core/Accordion';
import AccordionSummary from '@material-ui/core/AccordionSummary';
import AccordionDetails from '@material-ui/core/AccordionDetails';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

const useStyles = makeStyles((theme) => ({
  label: {
    display: 'flex',
    flexDirection: 'column',
  },
  labelValue: {
    fontSize: 12,
  }
}));

const FilterErrorSearch = () => {
  const classes = useStyles();
  const [value, setValue] = useState('2');

  const handleChange = (event) => {
    setValue(event.target.value);
  };

  const getNameSelected = (selected) => {
    return {
      1: 'Level',
      2: 'Descrição',
      3: 'Origem'
    }[selected];
  };

  return (
    <FormControl component="fieldset">
      <Accordion>
        <AccordionSummary
          expandIcon={<ExpandMoreIcon color="primary" />}
          aria-controls="panel1a-content"
          id="panel1a-header"
        >
          <div className={classes.label}>
            <FormLabel component="legend">Buscar por:</FormLabel>
            <Typography
              variant="body2"
              color="primary"
              className={classes.labelValue}
            >
              {getNameSelected(value)}
            </Typography>
          </div>
        </AccordionSummary>
        <AccordionDetails>
          <RadioGroup aria-label="orderLabel" name="order" value={value} onChange={handleChange}>
            <FormControlLabel
              control={<Radio color="primary" />}
              value="1"
              label="Level"
              labelPlacement="start"
            />
            <FormControlLabel
              control={<Radio color="primary" />}
              value="2"
              label="Descrição"
              labelPlacement="start"
            />
            <FormControlLabel
              control={<Radio color="primary" />}
              value="3"
              label="Origem"
              labelPlacement="start"
            />
          </RadioGroup>
        </AccordionDetails>
      </Accordion>
    </FormControl>
  );
}

export default FilterErrorSearch;
