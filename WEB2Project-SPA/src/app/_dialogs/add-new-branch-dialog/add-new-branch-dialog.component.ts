import { Component, Inject } from '@angular/core';
import { Branch } from 'src/app/_models/_shared/branch';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-add-new-branch-dialog',
  templateUrl: './add-new-branch-dialog.component.html',
  styleUrls: ['./add-new-branch-dialog.component.css']
})
export class AddNewBranchDialogComponent {
  newBranch: Branch = {
    city: '',
    country: '',
    address: '',
    mapString: '',
    id: 0
  };

  constructor(
    public dialogRef: MatDialogRef<AddNewBranchDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private rentalService: CarrentalService,
    private alertify: AlertifyService) {}

    onClose(): void {
      this.dialogRef.close();
   }

   onAdd() {
    this.rentalService.addNewBranch(this.data.id, this.newBranch).subscribe(res => { 
      this.alertify.success('Successfully added new branch!');
      this.dialogRef.close();
    }, error => {
      this.alertify.error('Problem while adding new branch!');
      this.dialogRef.close();
    });
  }

}
