import { Component, OnInit } from '@angular/core';
import { CarrentalService } from '../_services/carrental.service';

@Component({
  selector: 'app-rentacar-profile',
  templateUrl: './rentacar-profile.component.html',
  styleUrls: ['./rentacar-profile.component.css']
})
export class RentacarProfileComponent implements OnInit {

  constructor(private rentalService: CarrentalService) { }

  ngOnInit() {
    this.rentalService.getCarRentalCompany(3).subscribe(res => {
      console.log(res);
    });
  }

}
