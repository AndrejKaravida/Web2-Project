import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { CarCompany } from '../_models/carcompany';
import { AvioCompany } from '../_models/aviocompany';
import { AvioService } from '../_services/avio.service';
import { CarrentalService } from '../_services/carrental.service';
import { MatDialog } from '@angular/material/dialog';
import { AddNewCompanyDialogComponent } from '../_dialogs/_adminpanel_dialogs/add-new-company-dialog/add-new-company-dialog.component';
import { CompanyToMake } from '../_models/companytomake';
import { AlertifyService } from '../_services/alertify.service';
import { HttpClient } from '@angular/common/http';
import { CompanyAddSuccessfullDialogComponent } from '../_dialogs/_adminpanel_dialogs/company-add-successfull-dialog/company-add-successfull-dialog.component';
import { Router } from '@angular/router';
import { CompanyAdmin } from '../_models/_userModels/companyAdmin';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  displayedColumns: string[] = ['#', 'image', 'name', 'headOffice', 'averageGrade', 'bonusDiscount', 'administrator', 'profile'];
  dataSource: MatTableDataSource<CarCompany>;
  dataSource2: MatTableDataSource<AvioCompany>;
  newCompany: CompanyToMake = {
    name: '',
    city: '',
    country: '',
    mapString: ''
  };
  companyAdmin: CompanyAdmin = {
    email: '',
    password: '',
    firstName: '',
    lastName: '',
    companyId: 0,
    type: ''
  };

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild('secondPaginator') paginator2: MatPaginator;

  constructor(private rentalService: CarrentalService, private avioService: AvioService,
              private dialog: MatDialog, private alertify: AlertifyService,
              private http: HttpClient, private router: Router,
              private userService: UserService) { }

  ngOnInit() {
    this.getAllCarCompanies();
    this.getAllAvioCompanies();
  }

  getAllCarCompanies() {
    this.rentalService.getAllCarCompanies().subscribe(res => {
      this.dataSource = new MatTableDataSource(res);
      this.dataSource.paginator = this.paginator;
    });
  }

  getAllAvioCompanies() {
    this.avioService.getAllAvioCompanies().subscribe(res => {
      this.dataSource2 = new MatTableDataSource(res);
      this.dataSource2.paginator = this.paginator2;
    });
  }

  addNewCarCompany() {
    const dialogRef = this.dialog.open(AddNewCompanyDialogComponent, {
      width: '450px',
      height: '730px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
          this.newCompany.city = result.city;
          this.newCompany.country = result.country;
          this.newCompany.name = result.name;
          this.newCompany.mapString = result.mapString;
          this.companyAdmin.email = result.email;
          this.companyAdmin.firstName = result.firstName;
          this.companyAdmin.lastName = result.lastName;
          this.companyAdmin.password = result.password;
          this.companyAdmin.type = 'car';

          if (result.selectedFile == null || result.selectedFile === undefined) {
            alert('Please choose the photo!');
          } else {
            this.rentalService.makeNewCompany(this.newCompany).subscribe((response: any) => {
              const fd = new FormData();
              fd.append('file', result.selectedFile, result.selectedFile.name);
              this.companyAdmin.companyId = response.id;
              return this.http.post('http://localhost:5000/api/upload/newCompany/' + response.id, fd)
              .subscribe(res => {
                this.userService.createAdminUser(this.companyAdmin).subscribe(_ => {
                  this.getAllCarCompanies();
                  this.dialog.open(CompanyAddSuccessfullDialogComponent, {
                    width: '450px',
                    height: '200px',
                    data: {response}
                  });
                });
              }, error => {
                this.alertify.error('Error while adding new company!');
              });
            });
        }
      }
   });
  }

  addNewAvioCompany() {
    const dialogRef = this.dialog.open(AddNewCompanyDialogComponent, {
      width: '450px',
      height: '730px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.newCompany.city = result.city;
        this.newCompany.country = result.country;
        this.newCompany.name = result.name;
        this.newCompany.mapString = result.mapString;
        this.companyAdmin.email = result.email;
        this.companyAdmin.firstName = result.firstName;
        this.companyAdmin.lastName = result.lastName;
        this.companyAdmin.password = result.password;
        this.companyAdmin.type = 'avio';
  
        if (result.selectedFile == null || result.selectedFile === undefined) {
          alert('Please choose the photo!');
        } else {
          this.avioService.makeNewCompany(this.newCompany).subscribe((response: any) => {
            const fd = new FormData();
            fd.append('file', result.selectedFile, result.selectedFile.name);
            this.companyAdmin.companyId = response.id;
            return this.http.post('http://localhost:5000/api/upload/newAvioCompany/' + response.id, fd)
            .subscribe(res => {
              this.userService.createAdminUser(this.companyAdmin).subscribe(_ => {
                this.getAllAvioCompanies();
                this.dialog.open(CompanyAddSuccessfullDialogComponent, {
                  width: '450px',
                  height: '200px',
                  data: {response}
                });
              });
            }, error => {
              this.alertify.error('Error while adding new company!');
            });
          });
      }
      }
   });
  }

  goToAvioProfile(id: number) {
    this.router.navigate(['/avioprofile/' + id]);
  }

  goToCarProfile(id: number) {
    this.router.navigate(['/rentalprofile/' + id]);
  }


}
