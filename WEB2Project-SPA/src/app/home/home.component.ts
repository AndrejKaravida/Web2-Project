import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  destinations: any[] = [];

  constructor(public authService: AuthService) { }

  ngOnInit() {
    for(let i = 0; i < 10; i++){
      this.destinations.push('123');
    }
  }

}
