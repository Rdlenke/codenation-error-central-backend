import React, { useState, useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import api from '../../services/api';
import backIcon from '../../assets/icons/back.svg'
import './styles.css';

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
          <strong>Detalhes do erro:</strong>
        </div>
      </header>
    </div>
  );
}

export default DetailsError;