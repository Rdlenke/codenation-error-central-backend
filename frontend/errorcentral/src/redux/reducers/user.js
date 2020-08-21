import { ADD_USER } from '../actions/actionTypes';

const initialState = {
  email: "",
  token: "",
  guid: "",
  firstName: "",
  lastName: ""
}

export default (state = initialState, action) => {
  switch(action.type) {
    case ADD_USER: {
      const { email, token, guid, firstName, lastName } = action.payload;

      return {
        ...state,
        email,
        token,
        guid,
        firstName,
        lastName
      };
    }

    default:
      return state;
  }
}