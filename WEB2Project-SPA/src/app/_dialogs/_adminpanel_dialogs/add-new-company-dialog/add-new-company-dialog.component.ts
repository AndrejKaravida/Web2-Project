import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-add-new-company-dialog',
  templateUrl: './add-new-company-dialog.component.html',
  styleUrls: ['./add-new-company-dialog.component.css']
})
export class AddNewCompanyDialogComponent{

  constructor(
    public dialogRef: MatDialogRef<AddNewCompanyDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

    onNoClick(): void {
    this.dialogRef.close();
  }

  onFileSelected(event) {
    this.data.selectedFile = event.target.files[0] as File;
  }

}
