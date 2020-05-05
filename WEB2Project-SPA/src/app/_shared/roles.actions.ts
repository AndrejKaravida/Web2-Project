import { Action } from '@ngrx/store';

export const SET_ROLE = '[Roles] Set Role';
export const SET_NOROLE = '[Roles] Set NoRole';

export class SetRole implements Action {
  readonly type = SET_ROLE;

  constructor(public payload: string) {}
}

export class SetNoRole implements Action {
    readonly type = SET_NOROLE;
}

export type RoleActions = SetRole | SetNoRole;
