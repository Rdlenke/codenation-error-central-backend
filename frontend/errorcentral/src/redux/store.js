import { createStore, applyMiddleware, compose } from 'redux';
import thunk from 'redux-thunk';
import rootReducer from './reducers';
const initialState = {};

const composeEnhancer = window.__REDUX_EXTENSION_COMOSE__ || compose;
export default createStore(rootReducer, initialState, composeEnhancer(applyMiddleware(thunk)));