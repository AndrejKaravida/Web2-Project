import { ActionReducerMap, createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromAuth from './_shared/auth.reducer';
import * as fromChangePassword from './_shared/changePassword.reducer';

export interface State {
  auth: fromAuth.State;
  changePassword: fromChangePassword.State;
}

export const reducers: ActionReducerMap<State> = {
  auth: fromAuth.authReducer,
  changePassword: fromChangePassword.changePasswordReducer
};

export const getAuthState = createFeatureSelector<fromAuth.State>('auth');
export const getIsAuth = createSelector(getAuthState, fromAuth.getIsAuth);

export const getPasswordState = createFeatureSelector<fromChangePassword.State>('changePassword');
export const getDoesNeedToChancePassword = createSelector(getPasswordState, fromChangePassword.getDoesNeedToChangePassword);

