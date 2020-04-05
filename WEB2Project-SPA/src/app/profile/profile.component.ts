import { Component, OnInit,ViewChild } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { User } from '../_models/user';
import { faUser, faTimes } from '@fortawesome/free-solid-svg-icons';
import { MatButton } from '@angular/material/button';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';


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
  user: User;
  displayedColumns: string[] = ['name', 'surname', 'age', 'gender', 'button'];
  dataSource = new MatTableDataSource(this.friendList);

  constructor(public auth: AuthService) { }

  ngOnInit() {
    this.loadData();
  }


  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  loadData() {
    /*
    this.auth.userProfile$.subscribe(res => {
      console.log(res);
      this.user = res;
    });
    */
  }
  deleteTicket(rowid: number){

    if (rowid > -1) {
     this.friendList.splice(rowid, 1);
     this.dataSource = new MatTableDataSource(this.friendList);
  }
 
  }
}
