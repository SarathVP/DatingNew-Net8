import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule, RouterLink],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  model: any = {};
  accountService = inject(AccountService);
  private route = inject(Router);
  private toastr = inject(ToastrService);
  login() {
    this.accountService.login(this.model).subscribe({
      next: resp => {
        this.model = resp;
        this.route.navigateByUrl('/members')
      },
      error: err => {
        this.toastr.error(err.error);
      }
    })
  }

  logout() {
    this.accountService.logout();
    this.route.navigateByUrl('/');
  }
}
