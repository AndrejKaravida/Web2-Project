import { Component, OnInit, Input } from '@angular/core';
import { AvioCompany } from 'src/app/_models/_avioModels/aviocompany';
import { Router } from '@angular/router';

@Component({
  selector: 'app-aviocompany-card',
  templateUrl: './aviocompany-card.component.html',
  styleUrls: ['./aviocompany-card.component.css']
})
export class AviocompanyCardComponent implements OnInit {
  @Input() avioCompany: AvioCompany;

  constructor(private router: Router) { }

  ngOnInit() {
  }

  onProfileClick(){
    this.router.navigate(['avioprofile/' + this.avioCompany.id]);
  }


  
}
