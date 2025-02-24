import { HttpClient } from '@angular/common/http';
import { Component, inject, Inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
 
  title = 'datingClient';
  http = inject(HttpClient);

  users : any;

  ngOnInit(): void {
    this.http.get('http://localhost:5001/api/users').subscribe({
      next : resp => {
        this.users = resp;
        console.log(this.users);
      },
      error : err => {
        console.log(err);
      },
      complete : () =>{
        console.log("Request Completed");
      }
    })
  }
}
