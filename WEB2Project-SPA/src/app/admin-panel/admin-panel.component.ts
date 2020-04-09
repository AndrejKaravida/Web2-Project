import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { CarCompany } from '../_models/carcompany';
import { AvioCompany } from '../_models/aviocompany';
import { AvioService } from '../_services/avio.service';
import { CarrentalService } from '../_services/carrental.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  displayedColumns: string[] = ['#', 'image', 'name', 'headOffice', 'averageGrade', 'bonusDiscount', 'profile'];
  dataSource: MatTableDataSource<CarCompany>;
  dataSource2: MatTableDataSource<AvioCompany>;

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;

  constructor(private rentalService: CarrentalService, private avioService: AvioService) { }

  ngOnInit() {
    this.rentalService.getAllCarCompanies().subscribe(res => {
      this.dataSource = new MatTableDataSource(res);
      this.dataSource.paginator = this.paginator;
    });

    this.avioService.getAllAvioCompanies().subscribe(res => {
      this.dataSource2 = new MatTableDataSource(res);
      this.dataSource.paginator = this.paginator;
    });

  }

}
