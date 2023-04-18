import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();

  model: any = {};
  registerForm: FormGroup = new FormGroup({});

  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder) { };

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required,Validators.minLength(7)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],

    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
      })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}
        }
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
