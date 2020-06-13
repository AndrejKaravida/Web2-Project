import { Injectable } from '@angular/core';
import createAuth0Client from '@auth0/auth0-spa-js';
import Auth0Client from '@auth0/auth0-spa-js/dist/typings/Auth0Client';
import { from, of, Observable, BehaviorSubject, combineLatest, throwError } from 'rxjs';
import { tap, catchError, concatMap, shareReplay, take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import * as fromRoot from '../app.reducer';
import * as Auth from '../_shared/auth.actions';
import * as ChangePassword from '../_shared/changePassword.actions';
import * as Roles from '../_shared/roles.actions';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  auth0Client$ = (from(
    createAuth0Client({
      domain: 'pusgs.eu.auth0.com',
      client_id: '6RZ4TiNvvWWf6U67KYJpSbnLsZjTqySM',
      redirect_uri: 'http://localhost:4200/home',
      audience: 'myproject'
    })
  ) as Observable<Auth0Client>).pipe(
    shareReplay(1),
    catchError(err => throwError(err))
  );
  isAuthenticated$ = this.auth0Client$.pipe(take(1),
    concatMap((client: Auth0Client) => from(client.isAuthenticated())),
    tap(res => {
      this.loggedIn = res;
      if (res) {
        this.store.dispatch(new Auth.SetAuthenticated());
        this.userProfile$.subscribe(result => {
          if (result) {
            localStorage.setItem('authId', result.sub);
            this.userService.getUserRole(result.email).subscribe(response => {
              if (response) {
                this.store.dispatch(new Roles.SetRole(response.name));
              }
            });
            this.userService.getUser(result.email).subscribe(data => {
              if (data.needToChangePassword) {
                this.store.dispatch(new ChangePassword.SetNeedToChangePassword());
                this.router.navigate(['changepassword']);
              } else {
                this.store.dispatch(new ChangePassword.SetNotNeedToChangePassword());
              }
            });
          }
        });
      } else {
        this.store.dispatch(new Auth.SetUnauthenticated());
        this.store.dispatch(new Roles.SetNoRole());
      }
    })
  );
  handleRedirectCallback$ = this.auth0Client$.pipe(
    concatMap((client: Auth0Client) => from(client.handleRedirectCallback()))
  );
  private userProfileSubject$ = new BehaviorSubject<any>(null);
  userProfile$ = this.userProfileSubject$.asObservable();
  loggedIn: boolean = null;

  constructor(private router: Router, private store: Store<fromRoot.State>, private userService: UserService) {
    this.localAuthSetup();
    this.handleAuthCallback();
  }

  getUser$(options?): Observable<any> {
    return this.auth0Client$.pipe(
      concatMap((client: Auth0Client) => from(client.getUser(options))),
      tap(user => this.userProfileSubject$.next(user))
    );
  }

  private localAuthSetup() {
    const checkAuth$ = this.isAuthenticated$.pipe(
      concatMap((loggedIn: boolean) => {
        if (loggedIn) {
          return this.getUser$();
        }
        return of(loggedIn);
      })
    );
    checkAuth$.subscribe();
  }

  login(redirectPath: string = '/') {
    this.auth0Client$.subscribe((client: Auth0Client) => {
      client.loginWithRedirect({
        redirect_uri: 'http://localhost:4200/home',
        appState: { target: redirectPath }
      });
    });
  }

  private handleAuthCallback() {
    const params = window.location.search;
    if (params.includes('code=') && params.includes('state=')) {
      let targetRoute: string;
      const authComplete$ = this.handleRedirectCallback$.pipe(
        tap(cbRes => {
          targetRoute = cbRes.appState && cbRes.appState.target ? cbRes.appState.target : '/';
        }),
        concatMap(() => {
          return combineLatest([
            this.getUser$(),
            this.isAuthenticated$
          ]);
        })
      );
      authComplete$.subscribe(([user, loggedIn]) => {
        if (!user.email_verified) {
          this.logout();
          alert('You need to verify your email address before you can log in!');
        }
        this.router.navigate([targetRoute]);
      });
    }
  }

  logout() {
    this.auth0Client$.subscribe((client: Auth0Client) => {
      client.logout({
        client_id: '6RZ4TiNvvWWf6U67KYJpSbnLsZjTqySM',
        returnTo: 'http://localhost:4200/home'
      });
      this.store.dispatch(new Auth.SetUnauthenticated());
      this.store.dispatch(new Roles.SetNoRole());
      localStorage.removeItem('authId');
    });
  }

  getTokenSilently$(options?): Observable<string> {
    return this.auth0Client$.pipe(
      concatMap((client: Auth0Client) => from(client.getTokenSilently(options)))
    );
  }

}
