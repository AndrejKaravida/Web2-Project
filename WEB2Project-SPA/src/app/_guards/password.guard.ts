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

  constructor(private store: Store<fromRoot.State>, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean|UrlTree> | boolean {
    let needToChangePassword = false;

    this.store.select(fromRoot.getDoesNeedToChancePassword).pipe(
        tap(res => {
            needToChangePassword = res;
        })
    );

    return !needToChangePassword;
  }
}
