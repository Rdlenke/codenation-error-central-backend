import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

import api from '../../services/api';

import Errors from '../../mock/errors';
import backIcon from '../../assets/icons/back.svg'
import bugImg from '../../assets/images/BugImage.png';
import arquivarImg from '../../assets/images/Arquivar.png';


import './styles.css';

const DetailsError = () => {
  const [errors, setErrors] = useState({ data: [], loading: false, status: 0 });
  useEffect(() => {
    setErrors({ data: [], loading: true, status: 0 });
    api.get('v1/logerrors')
      .then(response => {})
      .catch(error => {
        // setErrors({ data: null, loading: false, status: error.request.status });
        setErrors({ data: Errors, loading: false, status: 0 });
      });
  }, []);

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

      <main>
        <article className="error-item">
          <header>
            <img src={bugImg} alt="error type"/>
            <div>
              <strong>Nome do erro</strong>
              <span>http://localhost:3000</span>
            </div>
          </header>
          <p>
            Detalhes do erro: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum at orci eget erat iaculis ultrices ut nec tortor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent ut leo ut lacus fermentum cur
          </p>

          <footer>
            <p>
              Identificador:
              <strong>id</strong>
            </p>
            <button type="button">
              <img src={arquivarImg} alt="arquivar"/>
            </button>
          </footer>
        </article>
      </main>

    </div>
  );
}

export default DetailsError;