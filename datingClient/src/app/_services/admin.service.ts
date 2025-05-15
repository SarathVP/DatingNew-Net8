import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.baseUrl;
  private http = inject(HttpClient);


  getUsersWithRole(){
    return this.http.get<User[]>(this.baseUrl + 'Admin/users-with-role');
  }

  updateUserRoles(username : string, roles : string[]){
    return this.http.post<string[]>(this.baseUrl + 'Admin/edit-roles/' + username + '?roles=' + roles, {});
  }
}
