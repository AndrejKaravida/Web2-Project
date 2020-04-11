import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-change-headoffice-dialog',
  templateUrl: './change-headoffice-dialog.component.html',
  styleUrls: ['./change-headoffice-dialog.component.css']
})
export class ChangeHeadofficeDialogComponent implements OnInit {
  headOffice: string;

  constructor(
    public dialogRef: MatDialogRef<ChangeHeadofficeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private rentalService: CarrentalService,
    private alertify: AlertifyService) {}

  ngOnInit() {
    this.headOffice = this.data.headOffice.city;
  }


  onClose(): void {
    this.dialogRef.close();
 }

  onChange() {
   this.rentalService.changeHeadOffice(this.data.id, this.headOffice).subscribe(res => { 
   }, error => {
    this.alertify.error('Failed to change head office!');
   });
   this.dialogRef.close();
  }

}
