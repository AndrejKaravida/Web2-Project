import { Component, OnInit } from '@angular/core';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/vehicle';
import { ActivatedRoute } from '@angular/router';
import { EditrentalcompanydialogComponent } from '../_dialogs/editrentalcompanydialog/editrentalcompanydialog.component';
import { CarCompany } from '../_models/carcompany';
import { MatDialog } from '@angular/material/dialog';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-rentacar-profile',
  templateUrl: './rentacar-profile.component.html',
  styleUrls: ['./rentacar-profile.component.css']
})
export class RentacarProfileComponent implements OnInit {
  rentalCompany: CarCompany;
  vehicles: Vehicle[];

  constructor(private rentalService: CarrentalService, private route: ActivatedRoute,
              private dialog: MatDialog, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadCompany();
  }

  loadCompany() {
    this.route.data.subscribe(data => {
      const key = 'carcompany';
      this.rentalCompany = data[key];
    });
  }

  onEditCompany() {
    this.openDialog();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(EditrentalcompanydialogComponent, {
      width: '400px',
      height: '630px',
      data: {...this.rentalCompany}
    });

    dialogRef.afterClosed().subscribe(result => {
      result.weekRentalDiscount = +result.weekRentalDiscount;
      result.monthRentalDiscount = +result.monthRentalDiscount;
      this.rentalService.updateComapny(result).subscribe(res => {
        this.alertify.success('Successfully changed company data!');
        this.loadCompany();
      });
      }, err => {
        this.alertify.error('Problem editing company data!');
      });
  }

}
