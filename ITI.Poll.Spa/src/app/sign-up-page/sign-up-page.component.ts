import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IdentityService } from '../identity.service';

@Component({
  selector: 'app-sign-up-page',
  templateUrl: './sign-up-page.component.html',
  styleUrls: ['./sign-up-page.component.css']
})
export class SignUpPageComponent {
  errorMessages: string[]

  signupForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    nickname: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]]
  })

  constructor(
    private readonly identityService: IdentityService,
    private readonly fb: FormBuilder,
    private readonly router: Router) { }

  onSubmit() {
    this.identityService.signUp(this.email.value, this.nickname.value, this.password.value)
      .subscribe(x => {
        if (typeof(x.errors) != 'undefined' && x.errors.length > 0) {
          this.errorMessages = ['Unable to sign up.']
        } else if (x.data.user.signIn.errors.length > 0) {
          this.errorMessages = x.data.user.signIn.errors.map(e => e.message);
        } else {
          this.router.navigate(['home']);
        }
      });
  }

  get email() { return this.signupForm.get('email'); }

  get nickname() { return this.signupForm.get('nickname'); }

  get password() { return this.signupForm.get('password'); }

}
