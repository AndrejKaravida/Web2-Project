import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-rentacaroptiondialog',
  templateUrl: './rentacaroptiondialog.component.html',
  styleUrls: ['./rentacaroptiondialog.component.css']
})
export class RentacaroptiondialogComponent {

  constructor(public dialogRef: MatDialogRef<RentacaroptiondialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any,
              private router: Router) { }


  routeToRentaACar() {
    this.router.navigate(['rentalprofile', this.data.id]);
    this.dialogRef.close();
  }

  close() { 
    this.dialogRef.close();
  }

}
