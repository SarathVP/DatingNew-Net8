import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/User';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient)
  baseUrl = environment.baseUrl;
  currentUser = signal<User | null>(null);

  login(model: any) {
    //console.log(model);
    return this.http.post<User>(this.baseUrl + 'Account/login', model).pipe(
      map(user => {
        this.setCurrentUser(user);
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'Account/register', model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user : User){
    localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
  }

  logout() {
    this.currentUser.set(null);
    localStorage.removeItem('user'); 
  }
}
