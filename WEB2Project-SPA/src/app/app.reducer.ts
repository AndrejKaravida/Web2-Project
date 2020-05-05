import { ActionReducerMap, createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromAuth from './_shared/auth.reducer';
import * as fromChangePassword from './_shared/changePassword.reducer';
import * as fromRoles from './_shared/roles.reducer';

export interface State {
  auth: fromAuth.State;
  changePassword: fromChangePassword.State;
  role: fromRoles.State;
}

export const reducers: ActionReducerMap<State> = {
  auth: fromAuth.authReducer,
  changePassword: fromChangePassword.changePasswordReducer,
  role: fromRoles.rolesReducer
};

export const getAuthState = createFeatureSelector<fromAuth.State>('auth');
export const getIsAuth = createSelector(getAuthState, fromAuth.getIsAuth);

export const getPasswordState = createFeatureSelector<fromChangePassword.State>('changePassword');
export const getDoesNeedToChancePassword = createSelector(getPasswordState, fromChangePassword.getDoesNeedToChangePassword);

export const getRolesState = createFeatureSelector<fromRoles.State>('role');
export const getRole = createSelector(getRolesState, fromRoles.getRole);

