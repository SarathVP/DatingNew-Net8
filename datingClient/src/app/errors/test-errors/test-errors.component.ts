import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './test-errors.component.html',
  styleUrl: './test-errors.component.css'
})
export class TestErrorsComponent {
  baseUrl = "http://localhost:5001/api/";
  private http = inject(HttpClient);
  validationErrors : string[] = [];

  get400Error() {
    this.http.get(this.baseUrl + 'Buggy/bad-request').subscribe({
      next: resp => {
        console.log(resp);
      },
      error: err => {
        console.log(err);
      }
    })
  }

  get401Error() {
    this.http.get(this.baseUrl + 'Buggy/Auth').subscribe({
      next: resp => {
        console.log(resp);
      },
      error: err => {
        console.log(err);
      }
    })
  }

  get404Error() {
    this.http.get(this.baseUrl + 'Buggy/not-found').subscribe({
      next: resp => {
        console.log(resp);
      },
      error: err => {
        console.log(err);
      }
    })
  }

  get500Error() {
    this.http.get(this.baseUrl + 'Buggy/server-error').subscribe({
      next: resp => {
        console.log(resp);
      },
      error: err => {
        console.log(err);
      }
    })
  }

  get400ValidationError() {
    this.http.post(this.baseUrl + 'Account/register', {}).subscribe({
      next: resp => {
        console.log(resp);
      },
      error: err => {
        console.log(err);
        this.validationErrors = err;
      }
    })
  }
}
