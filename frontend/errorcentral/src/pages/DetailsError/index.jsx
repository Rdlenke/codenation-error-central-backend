import React from 'react';
import { Link } from 'react-router-dom';

import Errors from '../../mock/errors';
import backIcon from '../../assets/icons/back.svg'
import logoImg from '../../assets/images/ErrorCentralLogo.png';

import './styles.css';

const DetailsError = () => {
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