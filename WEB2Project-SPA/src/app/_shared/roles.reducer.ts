import { RoleActions, SET_ROLE, SET_NOROLE } from './roles.actions';


export interface State {
  role: string;
}

const initialState: State = {
  role: ''
};

export function rolesReducer(state = initialState, action: RoleActions) {
  switch (action.type) {
    case SET_ROLE:
      return {
        role: action.payload
      };
    case SET_NOROLE:
      return {
        role: ''
      };
    default: {
      return state;
    }
  }
}

export const getRole = (state: State) => state.role;
