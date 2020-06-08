import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faStar } from '@fortawesome/free-solid-svg-icons';
import { Vehicle } from 'src/app/_models/_carModels/vehicle';
import { Store } from '@ngrx/store';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { MatDialog } from '@angular/material/dialog';
import * as fromRoot from '../../app.reducer';
import { Observable } from 'rxjs';
import { BookDiscountedVehicleDialogComponent } from 'src/app/_dialogs/_rent_a_car_dialogs/book-discounted-vehicle-dialog/book-discounted-vehicle-dialog.component';

@Component({
  selector: 'app-discounted-vehicle',
  templateUrl: './discounted-vehicle.component.html',
  styleUrls: ['./discounted-vehicle.component.css']
})
export class DiscountedVehicleComponent implements OnInit {
  @Input() vehicle: Vehicle;
  @Input() admin: boolean;
  @Output() clicked = new EventEmitter();
  @Output() clicked2 = new EventEmitter();
  @Output() clicked3 = new EventEmitter();
  @Output() clicked4 = new EventEmitter();
  faUser = faUser;
  faStar = faStar;
  isAuth$: Observable<boolean>;
  canEdit = true;


  constructor(private store: Store<fromRoot.State>, private rentalService: CarrentalService,
              private alertify: AlertifyService, private dialog: MatDialog)  {
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

  onBookVehicle() {
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
