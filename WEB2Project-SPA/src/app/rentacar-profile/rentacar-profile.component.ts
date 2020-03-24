import { Component, OnInit } from '@angular/core';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/vehicle';

@Component({
  selector: 'app-rentacar-profile',
  templateUrl: './rentacar-profile.component.html',
  styleUrls: ['./rentacar-profile.component.css']
})
export class RentacarProfileComponent implements OnInit {
  vehicles: Vehicle[];

  constructor(private rentalService: CarrentalService) { }

  ngOnInit() {
    this.rentalService.getCarRentalCompany(3).subscribe(res => {
      this.vehicles = res.vehicles;
     // console.log(res);
     // console.log(this.vehicles);
    });
  }

}
