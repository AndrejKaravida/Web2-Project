import { Component, OnInit, ViewChild } from '@angular/core';
import { Reservation } from '../_models/carreservation';
import { AuthService } from '../_services/auth.service';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/vehicle';
import { MatDialog } from '@angular/material/dialog';
import { RateVehicleDialogComponent } from '../_dialogs/rate-vehicle-dialog/rate-vehicle-dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent implements OnInit {
  displayedColumns: string[] = ['#', 'image', 'model', 'totalDays', 'daysLeft', 'startDate', 'endDate', 'totalPrice', 'status', 'rate'];
  columns: string[] = ['#', 'image', 'departure destination', 'arrival destination', 'company name', 'price', 'flightRate' ]
  dataSource: MatTableDataSource<Reservation>;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  constructor(private authService: AuthService, private rentalService: CarrentalService,
              private dialog: MatDialog) { }

  ngOnInit() {
    this.authService.userProfile$.subscribe(res => {
      if (res) {
        this.rentalService.getCarReservationsForUser(res.name).subscribe(response => {
          this.dataSource = new MatTableDataSource(response);
          this.dataSource.paginator = this.paginator;
        });
      }
      });
  }

  onRate(vehicle: Vehicle, companyName: string, companyId: string) {
    this.dialog.open(RateVehicleDialogComponent, {
      width: '400px',
      height: '670px',
      data: {vehicle, companyName, companyId}
    });
  }
}
