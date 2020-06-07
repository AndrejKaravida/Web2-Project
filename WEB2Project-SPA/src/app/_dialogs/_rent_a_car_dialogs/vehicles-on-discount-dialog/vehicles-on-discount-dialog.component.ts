import { Component, OnInit, Inject } from '@angular/core';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { Vehicle } from 'src/app/_models/_carModels/vehicle';
import { EditCarDialogComponent } from '../edit-car-dialog/edit-car-dialog.component';
import { ChangeVehicleLocationDialogComponent } from '../changeVehicleLocationDialog/changeVehicleLocationDialog.component';

@Component({
  selector: 'app-vehicles-on-discount-dialog',
  templateUrl: './vehicles-on-discount-dialog.component.html',
  styleUrls: ['./vehicles-on-discount-dialog.component.css']
})
export class VehiclesOnDiscountDialogComponent implements OnInit {
  vehiclesOnDiscount: Vehicle[];

  constructor( public dialogRef: MatDialogRef<VehiclesOnDiscountDialogComponent>,
               @Inject(MAT_DIALOG_DATA) public data: any, private rentalService: CarrentalService,
               private alertify: AlertifyService, private dialog: MatDialog) { }

  ngOnInit() {
    this.loadVehicles();
  }

  loadVehicles() {
    this.rentalService.getDiscountedVehiclesForCompany(this.data.id).subscribe(res => {
      this.vehiclesOnDiscount = res;
    }, error => {
      this.alertify.error('Failed to load vehicles on discount!');
    });
  }

  onEditVehicle(vehicle: Vehicle) {
    const dialogRef = this.dialog.open(EditCarDialogComponent, {
      width: '400px',
      height: '755px',
      data: {...vehicle}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.rentalService.editVehicle(result, this.data.id).subscribe(res => {
          this.alertify.success('Vehicle edited successfully!');
          this.loadVehicles();
        }, error => {
          let errorMessage = '';

          for (const err of error.error.errors) {
         errorMessage += err.message;
         errorMessage += '\n';
        }
          this.alertify.error(errorMessage);
        });
      }
    });
  }

  onRemoveVehicle(vehicle: Vehicle) {
    this.alertify.confirm('Are you sure you want to remove vehicle? This action cannot be undone!', () => {
      this.rentalService.removeVehicle(vehicle.id, this.data.id).subscribe(res => {
        this.alertify.success('Vehicle successfuly deleted!');
        this.loadVehicles();
      }, error => {
        let errorMessage = '';

        for (const err of error.error.errors) {
       errorMessage += err.message;
       errorMessage += '\n';
      }
        this.alertify.error(errorMessage);
      });
    });
  }

  onChangeVehicleLocation(vehicle: Vehicle) {
    const dialogRef = this.dialog.open(ChangeVehicleLocationDialogComponent, {
      width: '450px',
      height: '350px',
      data: {company: this.data.rentalCompany, vehicle}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.vehiclesOnDiscount.find(x => x.id === vehicle.id).currentDestination = result;
      }
   });
  }

  onCancel() {
    this.dialogRef.close();
  }

}
