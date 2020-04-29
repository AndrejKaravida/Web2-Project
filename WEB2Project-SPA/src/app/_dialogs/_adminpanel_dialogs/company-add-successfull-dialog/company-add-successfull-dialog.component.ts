import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-company-add-successfull-dialog',
  templateUrl: './company-add-successfull-dialog.component.html',
  styleUrls: ['./company-add-successfull-dialog.component.css']
})
export class CompanyAddSuccessfullDialogComponent  {

  constructor(
    public dialogRef: MatDialogRef<CompanyAddSuccessfullDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

  onClose(): void {
    this.dialogRef.close();
  }

}
