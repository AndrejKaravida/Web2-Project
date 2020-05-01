import { Action } from '@ngrx/store';

export const SET_NEED_TO_CHANGE_PASSWORD = '[Change_Password] Set Need To Change Password';
export const SET_NOT_NEED_TO_CHANGE_PASSWORD = '[Change_Password] Set Not Need To Change Password';

export class SetNeedToChangePassword implements Action {
  readonly type = SET_NEED_TO_CHANGE_PASSWORD;
}

export class SetNotNeedToChangePassword implements Action {
  readonly type = SET_NOT_NEED_TO_CHANGE_PASSWORD;
}

export type ChangePasswordActions = SetNeedToChangePassword | SetNotNeedToChangePassword;
