import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CarrentalService } from 'src/app/_services/carrental.service';

@Component({
  selector: 'app-remove-destinations-dialog',
  templateUrl: './remove-destinations-dialog.component.html',
  styleUrls: ['./remove-destinations-dialog.component.css']
})
export class RemoveDestinationsDialogComponent {
  chosenDestination: string;
  error = false;

  constructor(public dialogRef: MatDialogRef<RemoveDestinationsDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any, private alertify: AlertifyService,
              private rentalService: CarrentalService) { }

              onClose(): void {
    this.dialogRef.close();
 }

  onRemove() {
    if (this.chosenDestination === this.data.headOffice.city) {
      this.error = true;
      return;
    }
    this.rentalService.removeCompanyLocation(this.data.id, this.chosenDestination).subscribe(res => {
      this.alertify.success('Destination successfully removed!');
      this.dialogRef.close();
    }, error => {
      this.alertify.error('Failed to remove destination!');
    });
  }

}