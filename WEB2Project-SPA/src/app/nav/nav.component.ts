import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  emailverified;
  email: string;
  nickname: string;
  pictureUrl = '';

  constructor(public authService: AuthService) {}

  ngOnInit() {
    this.authService.userProfile$.subscribe(res => { 
   if(res) {
     this.emailverified = res.email_verified;
     this.email = res.email;
     this.nickname = res.nickname;
     this.pictureUrl = res.picture;
    }
  });
  }
}
