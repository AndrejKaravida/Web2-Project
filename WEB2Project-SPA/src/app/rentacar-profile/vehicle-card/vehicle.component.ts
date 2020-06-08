import { Component, Input, EventEmitter, Output, OnInit } from '@angular/core';
import { Vehicle } from 'src/app/_models/_carModels/vehicle';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../app.reducer';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

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
  @Output() clicked4 = new EventEmitter();
  @Input() disabled: boolean;
  @Input() admin: boolean;
  isAuth$: Observable<boolean>;
  canEdit = true;

  faUser = faUser;
  faStar = faStar;

  constructor(private store: Store<fromRoot.State>, private rentalService: CarrentalService,
              private alertify: AlertifyService)  {
  }

  ngOnInit() {
    this.isAuth$ = this.store.select(fromRoot.getIsAuth);
    this.isAuth$.subscribe(res => { 
      if (res) {
        this.rentalService.canEditVehicle(this.vehicle.id).subscribe(res => {
          this.canEdit = res;
        });
      }
    });
  }

  onViewDeal() {
    this.clicked.emit(this.vehicle);
  }

  onEditVehicle() {
    if (this.canEdit) {
      this.clicked2.emit(this.vehicle);
    } else {
      this.alertify.warning('You cannot edit this vehicle because it has active reservations');
    }
  }

  onChangeLocation() {
    this.clicked4.emit(this.vehicle);
  }

  onRemoveVehicle() {
    if (this.canEdit) {
      this.clicked3.emit(this.vehicle);
    } else {
      this.alertify.warning('You cannot remove this vehicle because it has active reservations');
    }
  }

}
