import { Component, OnInit, Input } from '@angular/core';
import { DiscountedVehicle } from 'src/app/_models/discountedvehicle';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faStar } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-discounted-vehicle',
  templateUrl: './discounted-vehicle.component.html',
  styleUrls: ['./discounted-vehicle.component.css']
})
export class DiscountedVehicleComponent implements OnInit {
  @Input() vehicle: DiscountedVehicle;
  faUser = faUser;
  faStar = faStar;

  constructor() { }

  ngOnInit() {
  }

  onBookVehicle(){

  }

}
