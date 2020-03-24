import { Component, OnInit } from '@angular/core';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/vehicle';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-rentacar-profile',
  templateUrl: './rentacar-profile.component.html',
  styleUrls: ['./rentacar-profile.component.css']
})
export class RentacarProfileComponent implements OnInit {
  vehicles: Vehicle[];
  name: string;
  promodesc: string;
  id: number;
  address: string;
  averageGrade: number;
  weeklyDiscount: number;
  monthlyDiscount: number;

  constructor(private rentalService: CarrentalService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.id = params['id'];
  });
    this.rentalService.getCarRentalCompany(this.id).subscribe(res => {
      this.vehicles = res.vehicles;
      this.name = res.name;
      this.promodesc = res.promoDescription;
      this.address = res.address;
      this.averageGrade = res.averageGrade;
      this.monthlyDiscount = res.monthRentalDiscount;
      this.weeklyDiscount = res.weekRentalDiscount;
     // console.log(res);
     // console.log(this.vehicles);
    });

  }

}
