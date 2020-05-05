import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { Destination } from 'src/app/_models/destination';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-add-new-destination-dialog',
  templateUrl: './add-new-destination-dialog.component.html',
  styleUrls: ['./add-new-destination-dialog.component.css']
})
export class AddNewDestinationDialogComponent {
  newDestination: Destination = {
    city: '',
    country: '',
    mapString: '',
    id: 0
  };

  constructor(
    public dialogRef: MatDialogRef<AddNewDestinationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private rentalService: CarrentalService,
    private alertify: AlertifyService) {}

  onClose(): void {
     this.dialogRef.close();
  }

  onAdd() {
    this.rentalService.addNewDestination(this.data.id, this.newDestination).subscribe(res => { 
      this.alertify.success('Successfully added new location!');
      this.dialogRef.close();
    }, error => {
      this.alertify.error('Problem while adding new location!');
      this.dialogRef.close();
    });
  }

}
