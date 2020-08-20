import  { ADD_USER } from './actionTypes';

export const addUser = content => ({
    type: ADD_USER,
    payload: content
})