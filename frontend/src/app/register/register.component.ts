import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();

  model: any = {};

  constructor(private accountService: AccountService, private toastr: ToastrService) { };

  ngOnInit(): void {

  }

  register() {
    this.accountService.register(this.model).subscribe({
      next: () => {
        this.cancel();
      },
      error: errorResp => {
        if (errorResp.error.errors.Username) {
          this.toastr.error(errorResp.error.errors.Username);
        }
        if (errorResp.error.errors.Password) {
          this.toastr.error(errorResp.error.errors.Password);
        }
      }
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
