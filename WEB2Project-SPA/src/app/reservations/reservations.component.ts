import { Component, OnInit } from '@angular/core';
import { Reservation } from '../_models/carreservation';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/vehicle';
import { MatDialog } from '@angular/material/dialog';
import { RateVehicleDialogComponent } from '../_dialogs/editrentalcompanydialog/rate-vehicle-dialog/rate-vehicle-dialog.component';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent implements OnInit {
  reservations: Reservation[];

  constructor(private route: ActivatedRoute, private authService: AuthService, 
              private rentalService: CarrentalService,private dialog: MatDialog) { }

  ngOnInit() {

      this.authService.userProfile$.subscribe(res => {
      if (res) {
        this.rentalService.getCarReservationsForUser(res.name).subscribe(response => {
          this.reservations = response;
        });
      }
      });
 
  }

  onRate(vehicle: Vehicle, companyName: string, companyId: string) {
    const dialogRef = this.dialog.open(RateVehicleDialogComponent, {
      width: '400px',
      height: '630px',
      data: {vehicle, companyName, companyId}
    });
  }

  

}
