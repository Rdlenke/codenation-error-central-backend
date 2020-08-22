import axios from 'axios';

const api = axios.create({
  baseURL: 'https://error-central-codenation-g2.azurewebsites.net/api/',
});


export default api;