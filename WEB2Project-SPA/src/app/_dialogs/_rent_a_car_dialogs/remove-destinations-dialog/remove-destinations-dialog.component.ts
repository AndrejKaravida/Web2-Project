import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CarrentalService } from 'src/app/_services/carrental.service';

@Component({
  selector: 'app-remove-destinations-dialog',
  templateUrl: './remove-destinations-dialog.component.html',
  styleUrls: ['./remove-destinations-dialog.component.css']
})
export class RemoveDestinationsDialogComponent implements OnInit {
  chosenDestination: string;
  error = false;
  vehiclesError = false;

  constructor(public dialogRef: MatDialogRef<RemoveDestinationsDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any, private alertify: AlertifyService,
              private rentalService: CarrentalService) { }

  ngOnInit() {
   this.chosenDestination = this.data.headOffice.city;
  }

  onClose(): void {
    this.dialogRef.close();
 }

  onRemove() {
    if (this.chosenDestination === this.data.headOffice.city) {
      this.error = true;
      this.vehiclesError = false;
      return;
    } else {
      this.error = false;
      this.rentalService.canRemoveLocation(this.data.id, this.chosenDestination).subscribe(res => {
        if (!res) {
          this.vehiclesError = true;
        } else {
          this.rentalService.removeCompanyLocation(this.data.id, this.chosenDestination).subscribe(res => {
            this.alertify.success('Destination successfully removed!');
            this.dialogRef.close();
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
  }
}
