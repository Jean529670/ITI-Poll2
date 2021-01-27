import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IdentityService } from '../identity.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  errorMessages: string[]

  loginForm = this.fb.group({
    login: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  })

  constructor(
    private readonly fb: FormBuilder,
    private readonly identityService: IdentityService,
    private readonly router: Router) { }
  
  onSubmit() {
    this.identityService
      .authenticate(this.login.value, this.password.value)
      .subscribe(x => {
        if (typeof(x.errors) != 'undefined' && x.errors.length > 0) {
          this.errorMessages = ['Unable to sign in.']
        } else if (x.data.user.signIn.errors.length > 0) {
          this.errorMessages = x.data.user.signIn.errors.map(e => e.message);
        } else {
          this.router.navigate(['home']);
        }
      });
  }

  get login() { return this.loginForm.get('login'); }

  get password() { return this.loginForm.get('password'); }
}
