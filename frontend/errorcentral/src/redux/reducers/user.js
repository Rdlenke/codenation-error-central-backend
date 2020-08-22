import { ADD_USER, REMOVE_USER } from '../actions/actionTypes';

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
        case REMOVE_USER: {
            return initialState;
        }
        default:
            return state;
    }
}