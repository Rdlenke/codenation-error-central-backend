import React, { useState, useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import api from '../../services/api';
import Grid from '@material-ui/core/Grid';
import styled, { css } from "styled-components";
import backIcon from '../../assets/icons/back.svg'
import './styles.css';

const Detalhes = styled.span`
  font-family: Roboto;
  font-style: normal;
  font-weight: 400;
  color: #121212;
  font-size: 108px;
`;

const Rect = styled.div`
  width: 211px;
  height: 39px;
  margin-left: 914px;
  margin-top: 45px;
`;

const DetalhesRow = styled.div`
  height: 130px;
  flex-direction: row;
  display: flex;
  margin-top: 39px;
  margin-left: 69px;
  margin-right: 372px;
`;

const Rect2 = styled.div`
  width: 122px;
  height: 44px;
  margin-left: 1403px;
  font-family: Roboto;
  font-style: normal;
  font-weight: 400;
  color: rgba(125,119,119,1);
`;

const Rect3 = styled.div`
  width: 999px;
  height: 518px;
  font-family: Roboto;
  font-style: normal;
  font-weight: 400;
  color: rgba(125,119,119,1);
  font-size: 25px;
`;

const Rect4 = styled.div`
  width: 231px;
  height: 226px;
  margin-left: 293px;
  margin-top: 65px;
`;

const Rect3Row = styled.div`
  height: 518px;
  flex-direction: row;
  display: flex;
  margin-top: 25px;
  margin-left: 69px;
  margin-right: 305px;
`;

const DetailsError = (props) => {
  let { id } = props.params;

  const [errors, setErrors] = useState({ data: [], loading: false, status: 0 });

  useEffect(() => {
    setErrors({ data: [], loading: true, status: 0 });

    api.get(`v1/logerrors/${id}`)
      .then(response => {
        setErrors({ data: response.data.data, loading: false, status: 0});
        console.log(response.data.data);
      })
      .catch(error => {
        // setErrors({ data: null, loading: false, status: error.request.status });
        setErrors({ data: error, loading: false, status: 0 });
      });
  }, [id]);

  return (
    <div id="page-details-error" className="container">
      <header className="page-header">
        <div className="top-bar-container">
          <Link to="/">
            <img src={backIcon} alt="Voltar" />
          </Link>
          <strong>
            Error Central API
          </strong>
        </div>

        <div className="header-content">
          <strong>Erro no {errors.data.source}</strong>
        </div>
      </header>
      <div>
        <div className="detailsText">
          <span>Detalhes:</span>
        </div>
        <div className="detailsContainer">
          <span>{errors.data.details}</span>
        </div>
       </div>
    </div>
  );
}

export default DetailsError;