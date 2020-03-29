import { Component, OnInit, Input } from '@angular/core';
import { AvioCompany } from 'src/app/_models/aviocompany';
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
    this.router.navigate(['aviocompany/' + this.avioCompany.id]);
  }

}
