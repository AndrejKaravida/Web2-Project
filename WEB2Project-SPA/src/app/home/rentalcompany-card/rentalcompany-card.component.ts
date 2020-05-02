import { Component, OnInit, Input } from '@angular/core';
import { CarCompany } from 'src/app/_models/_carModels/carcompany';
import { Router } from '@angular/router';

@Component({
  selector: 'app-rentalcompany-card',
  templateUrl: './rentalcompany-card.component.html',
  styleUrls: ['./rentalcompany-card.component.css']
})
export class RentalcompanyCardComponent implements OnInit {
  @Input() carCompany: CarCompany;

  constructor(private router: Router) { }

  ngOnInit() {
  }

  onProfileClick(){
    this.router.navigate(['rentalprofile/' + this.carCompany.id]);
  }

}
