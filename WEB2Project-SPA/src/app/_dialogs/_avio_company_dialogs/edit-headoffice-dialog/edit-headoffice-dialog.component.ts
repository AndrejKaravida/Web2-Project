import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AvioService } from 'src/app/_services/avio.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Destination } from 'src/app/_models/_avioModels/destination';

@Component({
  selector: 'app-edit-headoffice-dialog',
  templateUrl: './edit-headoffice-dialog.component.html',
  styleUrls: ['./edit-headoffice-dialog.component.css']
})
export class EditHeadofficeDialogComponent {
  headOffice: string;
  constructor(public dialogRef: MatDialogRef<EditHeadofficeDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any, private avioService: AvioService,
              private alertify: AlertifyService) { }

  onClose(): void {
    this.dialogRef.close();
 }

  onChange() {
    this.avioService.editHeadOffice(this.data.id, this.headOffice).subscribe(res => {
     this.alertify.success('Head office location successfully changed!');
   }, error => {
    this.alertify.error('Failed to change head office!');
   });
    this.dialogRef.close();
  }
}
