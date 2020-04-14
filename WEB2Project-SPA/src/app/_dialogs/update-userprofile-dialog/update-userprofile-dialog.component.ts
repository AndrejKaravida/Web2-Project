import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-update-userprofile-dialog',
  templateUrl: './update-userprofile-dialog.component.html',
  styleUrls: ['./update-userprofile-dialog.component.css']
})
export class UpdateUserprofileDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<UpdateUserprofileDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }
 
}
