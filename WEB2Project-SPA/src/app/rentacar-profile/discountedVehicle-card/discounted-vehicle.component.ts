import { Component, OnInit, Input } from '@angular/core';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import { Vehicle } from 'src/app/_models/_carModels/vehicle';

@Component({
  selector: 'app-discounted-vehicle',
  templateUrl: './discounted-vehicle.component.html',
  styleUrls: ['./discounted-vehicle.component.css']
})
export class DiscountedVehicleComponent implements OnInit {
  @Input() vehicle: Vehicle;
  faUser = faUser;
  faStar = faStar;

  constructor() { }

  ngOnInit() {
  }

  onBookVehicle(){

  }

}
