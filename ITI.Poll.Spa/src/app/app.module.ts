import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from './home-page/home-page.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { identityReducer } from './identity.reducer';
import { ReactiveFormsModule } from '@angular/forms';
import { SignUpPageComponent } from './sign-up-page/sign-up-page.component';
import { NewPollPageComponent } from './new-poll-page/new-poll-page.component';
import { PollPageComponent } from './poll-page/poll-page.component';
import { PollComponent } from './poll/poll.component';
import { InvitationPageComponent } from './invitation-page/invitation-page.component';
import { InvitationFormComponent } from './invitation-form/invitation-form.component';
import { MeComponent } from './me/me.component';

@NgModule({
  declarations: [
    AppComponent,
    HomePageComponent,
    LoginPageComponent,
    SignUpPageComponent,
    NewPollPageComponent,
    PollPageComponent,
    PollComponent,
    InvitationPageComponent,
    InvitationFormComponent,
    MeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    StoreModule.forRoot({ identity: identityReducer })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
