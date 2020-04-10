import { Component, Input, EventEmitter, Output } from '@angular/core';
import { Vehicle } from 'src/app/_models/vehicle';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faStar } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html',
  styleUrls: ['./vehicle.component.css']
})
export class VehicleComponent {
  @Input() vehicle: Vehicle;
  @Output() clicked = new EventEmitter();
  @Output() clicked2 = new EventEmitter();
  @Output() clicked3 = new EventEmitter();

  faUser = faUser;
  faStar = faStar;

  constructor() {}

  onViewDeal() {
    this.clicked.emit(this.vehicle);
  }

  onEditVehicle() {
    this.clicked2.emit(this.vehicle);
  }

  onRemoveVehicle() { 
    this.clicked3.emit(this.vehicle);
  }


}
