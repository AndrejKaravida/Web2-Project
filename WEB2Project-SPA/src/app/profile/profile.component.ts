import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { User } from '../_models/_userModels/user';
import { faUser, faTimes } from '@fortawesome/free-solid-svg-icons';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { UserMetadata } from '../_models/_userModels/userMetadata';
import { UserService } from '../_services/user.service';
import { UserToUpdate } from '../_models/_userModels/userToUpdate';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UpdatePassword } from '../_models/_userModels/updatePassword';
import { CustomValidators } from '../custom-validators';

export interface Friends {
  name: string;
  surname: string;
  age: number;
  gender: string;
}

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  friendList: Friends[] = [
    {name: 'Goran', surname: 'Karavida', age: 22, gender: 'M'},
    {name: 'John', surname: 'Fox', age: 33, gender: 'M'},
    {name: 'Ivana', surname: 'Smith', age: 63, gender: 'F'},
    {name: 'Mickey', surname: 'Rooney', age: 24, gender: 'M'},
    {name: 'Peter', surname: 'Dempsey', age: 12, gender: 'M'},
    {name: 'Jack', surname: 'Boron', age: 43, gender: 'M'},
    {name: 'Nina', surname: 'Nickolson', age: 23, gender: 'F'},
  ];

  faUser = faUser;
  fa = faTimes;

  userMeta: UserMetadata = {
    first_name: '',
    last_name: '',
    phone_number: '',
    city: ''
  };
  userToSend: UserToUpdate = {
    email: '',
    user_metadata: this.userMeta
  };

  korisnik: User = {
    id: '',
    email: '',
    firstName: '',
    lastName: '',
    city: '',
    phoneNumber: '',
    needToChangePassword: false
  };

  formGroup: FormGroup;
  updatePassword: UpdatePassword = {
    email: '',
    password: ''
  };


  displayedColumns: string[] = ['name', 'surname', 'age', 'gender', 'button'];
  dataSource = new MatTableDataSource(this.friendList);


  constructor(public auth: AuthService, private alertify: AlertifyService,
              private userService: UserService, private formBuilder: FormBuilder,) { }

  ngOnInit() {
    this.loadKorisnik();

    this.formGroup = this.formBuilder.group({
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        CustomValidators.patternValidator(/\d/, { hasNumber: true }),
        CustomValidators.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
        CustomValidators.patternValidator(/[a-z]/, { hasSmallCase: true }),
       ]],
       confirmPassword: ['', Validators.required]
    }, {validator: CustomValidators.passwordMatchValidator});
  }

  loadKorisnik() {
    this.auth.userProfile$.subscribe(result => {
      this.userService.getUser(result.email).subscribe(res => {
        this.korisnik = res;
      });
    });

  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  deleteTicket(rowid: number) {
    if (rowid > -1) {
     this.friendList.splice(rowid, 1);
     this.dataSource = new MatTableDataSource(this.friendList);
  }
  }

  updatePass() {
    this.updatePassword.email = this.korisnik.email;
    this.updatePassword.password = this.formGroup.get('password').value;

    this.userService.updatePassword(this.updatePassword).subscribe(res => {
      this.alertify.success('You have successfully changed your password');
    }, error => { 
      this.alertify.error('There was an error updating your password');
    });
  }

  UpdateProfile() {
    this.userMeta.first_name = this.korisnik.firstName;
    this.userMeta.last_name = this.korisnik.lastName;
    this.userMeta.city = this.korisnik.city;
    this.userMeta.phone_number = this.korisnik.phoneNumber;

    this.userToSend.user_metadata = this.userMeta;
    this.userToSend.email = this.korisnik.email;

    this.userService.updateMetadata(this.userToSend).subscribe(res => {
      this.alertify.success('Successfully updated profile!');
      this.userService.getUser(this.korisnik.email).subscribe(result => {
        this.korisnik = result;
      });
    }, err => {
      this.alertify.success('There was a problem updating user data');
    });
  }

 

}
