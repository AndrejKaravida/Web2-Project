import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarCompany } from 'src/app/_models/_carModels/carcompany';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-changeVehicleLocationDialog',
  templateUrl: './changeVehicleLocationDialog.component.html',
  styleUrls: ['./changeVehicleLocationDialog.component.css']
})
export class ChangeVehicleLocationDialogComponent implements OnInit {
  newCity: string;

  constructor(
    public dialogRef: MatDialogRef<ChangeVehicleLocationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private rentalService: CarrentalService,
    private alertify: AlertifyService) {}

  ngOnInit() {
    this.newCity = this.data.company.currentDestination;
  }

  onClose(): void {
    this.dialogRef.close();
 }

  onChange() {
    if (this.newCity === this.data.vehicle.currentDestination) {
      this.dialogRef.close();
      return;
    }

    this.rentalService.changeVehicleLocation(this.data.vehicle.id, this.newCity, this.data.company.id).subscribe(res => {
     this.alertify.success('Vehicle location successfully changed!');
   }, error => {
    this.alertify.error('Failed to change vehicle location!');
   });
    this.dialogRef.close(this.newCity);
  }
}
