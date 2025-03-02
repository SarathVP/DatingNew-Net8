import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  model: any = {};
  accountService = inject(AccountService)

  login() {
    this.accountService.login(this.model).subscribe({
      next: resp => {
        this.model = resp;
        //console.log(this.model);
      },
      error: err => {
        console.log(err);
      }
    })
  }

  logout() {
    this.accountService.logout();
  }
}
