import { Component, EventEmitter, inject, input, Output, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  model: any = {};
  cancelRegister = output<boolean>();
  accountService = inject(AccountService);
  private toastr = inject(ToastrService);

  register() {
    this.accountService.register(this.model).subscribe({
      next: resp => {
        this.model = resp;
        this.cancel();
      },
      error: err => {
        this.toastr.error(err.error);
      }
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
