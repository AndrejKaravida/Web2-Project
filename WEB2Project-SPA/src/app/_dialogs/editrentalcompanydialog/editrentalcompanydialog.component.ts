import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarCompany } from 'src/app/_models/carcompany';

@Component({
  selector: 'app-editrentalcompanydialog',
  templateUrl: './editrentalcompanydialog.component.html',
  styleUrls: ['./editrentalcompanydialog.component.css']
})
export class EditrentalcompanydialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<EditrentalcompanydialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CarCompany) {}

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
