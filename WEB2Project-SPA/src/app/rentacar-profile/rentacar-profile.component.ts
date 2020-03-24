import { Component, OnInit } from '@angular/core';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/vehicle';
import { ActivatedRoute } from '@angular/router';
import { FormControl } from '@angular/forms';
import { EditrentalcompanydialogComponent } from '../_dialogs/editrentalcompanydialog/editrentalcompanydialog.component';
import { CarCompany } from '../_models/carcompany';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-rentacar-profile',
  templateUrl: './rentacar-profile.component.html',
  styleUrls: ['./rentacar-profile.component.css']
})
export class RentacarProfileComponent implements OnInit {
  rentalCompany: CarCompany;
  vehicles: Vehicle[];
  name: string;
  promodesc: string;
  id: number;
  address: string;
  averageGrade: number;
  weeklyDiscount: number;
  monthlyDiscount: number;

  constructor(private rentalService: CarrentalService, private route: ActivatedRoute,
              private dialog: MatDialog) { }

  openDialog(): void {
    const dialogRef = this.dialog.open(EditrentalcompanydialogComponent, {
      width: '400px',
      height: '600px',
      data: {company: this.rentalCompany}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.id = params['id'];
    });

    this.rentalService.getCarRentalCompany(this.id).subscribe(res => {
      this.rentalCompany = res;
      this.vehicles = res.vehicles;
      this.name = res.name;
      this.promodesc = res.promoDescription;
      this.address = res.address;
      this.averageGrade = res.averageGrade;
      this.monthlyDiscount = res.monthRentalDiscount;
      this.weeklyDiscount = res.weekRentalDiscount;
    });
  }

  onEditCompany(){
    this.openDialog();
  }

}
