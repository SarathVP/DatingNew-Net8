import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/Pagination';
import { UserParams } from '../_models/UserParams';
import { AccountService } from './account.service';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl = environment.baseUrl;
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null)
  MemberCaChe = new Map();
  user = this.accountService.currentUser();
  userParams = signal<UserParams>(new UserParams(this.user));


  resetUserParams(){
    this.userParams.set(new UserParams(this.user));
  }

  getAllMembers(){
    const response = this.MemberCaChe.get(Object.values(this.userParams()).join('-'));
    if (response) return setPaginatedResponse(response, this.paginatedResult);

    let params = setPaginationHeaders(this.userParams().pageNumber, this.userParams().pageSize);

    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy);

    return this.http.get<Member[]>(this.baseUrl + 'Users', {observe :'response', params}).subscribe({
      next : response => {
        setPaginatedResponse(response, this.paginatedResult);
        this.MemberCaChe.set(Object.values(this.userParams()).join('-'),response);
      }
    })
  }

  

  getMember(username : string){
    const member : Member = [...this.MemberCaChe.values()]
      .reduce((arr, elem) => arr.concat(elem.body),[])
      .find((m : Member) => m.userName == username);
    if (member) return of(member);

    return this.http.get<Member>(this.baseUrl + 'Users/' + username);
  }

  UpdateMember(member : Member){
    return this.http.put(this.baseUrl + 'Users', member).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => m.userName == member.userName ? member : m))
      // })
    );
  }

  setMainPhoto(photo : Photo){
    return this.http.put(this.baseUrl + 'Users/set-main-photo/' + photo.id, {}).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if (m.photos.includes(photo)) {
      //       m.photoUrl = photo.url
      //     }
      //     return m;
      //   }) )
      // })
    )
  }

  deletePhoto(photo : Photo){
    return this.http.delete(this.baseUrl + 'Users/delete-photo/' + photo.id, {}).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if (m.photos.includes(photo)) {
      //       m.photos = m.photos.filter(x => x.id != photo.id)
      //     }
      //     return m;
      //   }))
      // })
    )
  }
}
