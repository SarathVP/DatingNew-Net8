import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Message } from '../_models/Message';
import { PaginatedResult } from '../_models/Pagination';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.baseUrl;
  private http = inject(HttpClient);
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);


  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = setPaginationHeaders(pageNumber, pageSize);

    params = params.append('Container', container);

    return this.http.get<Message[]>(this.baseUrl + "Messages", { observe: 'response', params })
      .subscribe({
        next: response => setPaginatedResponse(response, this.paginatedResult)
      })
  }

  getMessageThread(username : string){
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
  }

  sendMessage(username : string, content : string){
    return this.http.post<Message>(this.baseUrl + 'Messages', {RecipientUsername : username, Content : content});
  }

  deleteMessage(id : number){
    return this.http.delete(this.baseUrl + 'Messages/' +  id);
  }
}
