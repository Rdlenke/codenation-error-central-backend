import  { ADD_USER, REMOVE_USER } from './actionTypes';

export const addUser = content => ({
    type: ADD_USER,
    payload: content
})

export const removeUser = () => ({
    type: REMOVE_USER,
})