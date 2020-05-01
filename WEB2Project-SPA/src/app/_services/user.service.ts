import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/_userModels/user';
import { Observable } from 'rxjs';
import { UserToUpdate } from '../_models/_userModels/userToUpdate';
import { UpdatePassword } from '../_models/_userModels/updatePassword';
import { CompanyAdmin } from '../_models/_userModels/companyAdmin';
import { UserRole } from '../_models/_userModels/userRole';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + 'auth/';

  constructor(private http: HttpClient) { }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'getUsers');
  }

  getUser(email: string): Observable<User> {
    return this.http.post<User>(this.baseUrl + 'getUserByEmail', {email});
  }

  updateMetadata(userToUpdate: UserToUpdate) {
    return this.http.post(this.baseUrl + 'updateUserMetadata', userToUpdate);
  }

  updatePassword(updatePassword: UpdatePassword) {
    return this.http.post(this.baseUrl + 'updatePassword', updatePassword);
  }

  createAdminUser(companyAdmin: CompanyAdmin) {
    return this.http.post(this.baseUrl + 'createAdminUser', companyAdmin);
  }

  getUserRole(email: string): Observable<UserRole> {
    return this.http.post<UserRole>(this.baseUrl + 'getUserRole', {email});
  }
}
