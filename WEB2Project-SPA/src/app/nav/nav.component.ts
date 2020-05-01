import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import * as fromRoot from '../app.reducer';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  emailverified;
  email: string;
  nickname: string;
  pictureUrl = '';
  isAuth$: Observable<boolean>;
  needToChangePassword$: Observable<boolean>;
  isSysAdmin = false;

  constructor(public authService: AuthService, private store: Store<fromRoot.State>) {}

  ngOnInit() {
    this.isAuth$ = this.store.select(fromRoot.getIsAuth);
    this.needToChangePassword$ = this.store.select(fromRoot.getDoesNeedToChancePassword);
    this.store.select(fromRoot.getRole).subscribe(res => {
      if (res === 'sysadmin') {
        this.isSysAdmin = true;
      }
    });

    this.authService.userProfile$.subscribe(res => { 
   if (res) {
     this.emailverified = res.email_verified;
     this.email = res.email;
     this.nickname = res.nickname;
     this.pictureUrl = res.picture;
    }
  });
  }
}
