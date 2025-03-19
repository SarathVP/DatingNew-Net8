import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.baseUrl;

  getAllMembers(){
    return this.http.get<Member[]>(this.baseUrl + 'Users');
  }

  getMember(username : string){
    return this.http.get<Member>(this.baseUrl + 'Users/' + username);
  }
}
