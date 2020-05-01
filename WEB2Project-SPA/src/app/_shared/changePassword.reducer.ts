import { ChangePasswordActions, SET_NEED_TO_CHANGE_PASSWORD, SET_NOT_NEED_TO_CHANGE_PASSWORD } from './changePassword.actions';

export interface State {
  doesNeedToChangePassword: boolean;
}

const initialState: State = {
    doesNeedToChangePassword: false
};

export function changePasswordReducer(state = initialState, action: ChangePasswordActions) {
  switch (action.type) {
    case SET_NEED_TO_CHANGE_PASSWORD:
      return {
        doesNeedToChangePassword: true
      };
    case SET_NOT_NEED_TO_CHANGE_PASSWORD:
      return {
        doesNeedToChangePassword: false
      };
    default: {
      return state;
    }
  }
}

export const getDoesNeedToChangePassword = (state: State) => state.doesNeedToChangePassword;
