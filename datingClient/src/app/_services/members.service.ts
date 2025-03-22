import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.baseUrl;
  members = signal<Member[]>([]);

  getAllMembers(){
    return this.http.get<Member[]>(this.baseUrl + 'Users').subscribe({
      next : response => {
        this.members.set(response);
      }
    })
  }

  getMember(username : string){
    const member = this.members().find(x => x.userName == username);
    if (member !== undefined) return of(member);

    return this.http.get<Member>(this.baseUrl + 'Users/' + username);
  }

  UpdateMember(member : Member){
    return this.http.put(this.baseUrl + 'Users', member);
  }
}
