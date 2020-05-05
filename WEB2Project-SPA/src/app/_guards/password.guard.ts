import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import * as fromRoot from '../app.reducer';

@Injectable({
  providedIn: 'root'
})
export class PasswordGuard implements CanActivate {
  needToChangePassword = false;

  constructor(private store: Store<fromRoot.State>) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean|UrlTree> | boolean {
  
    this.store.select(fromRoot.getDoesNeedToChancePassword).pipe(
        tap(res => {
            this.needToChangePassword = res;
        })
    );

    return !this.needToChangePassword;
  }
}
