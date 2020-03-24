import { Component, OnInit, Input } from '@angular/core';
import { Vehicle } from 'src/app/_models/vehicle';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faStar } from '@fortawesome/free-solid-svg-icons';
@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html',
  styleUrls: ['./vehicle.component.css']
})
export class VehicleComponent implements OnInit {
  @Input() vehicle: Vehicle;
  faUser = faUser;
  faStar = faStar;

  constructor() { }

  ngOnInit() {
    console.log(this.vehicle);
  }

}
