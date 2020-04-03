import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
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
  @Output() clicked = new EventEmitter();
  @Output() clicked2 = new EventEmitter();
  @Output() clicked3 = new EventEmitter();

  faUser = faUser;
  faStar = faStar;

  constructor() { }

  ngOnInit() {
  }

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
