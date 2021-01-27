import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { HomePageComponent } from './home-page/home-page.component';
import { InvitationPageComponent } from './invitation-page/invitation-page.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { NewPollPageComponent } from './new-poll-page/new-poll-page.component';
import { PollPageComponent } from './poll-page/poll-page.component';
import { SignUpPageComponent } from './sign-up-page/sign-up-page.component';

const routes: Routes = [
  { path: 'home', component: HomePageComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginPageComponent },
  { path: 'signup', component: SignUpPageComponent },
  { path: 'new-poll', component: NewPollPageComponent, canActivate: [AuthGuard] },
  { path: 'poll/:pollId', component: PollPageComponent, canActivate: [AuthGuard] },
  { path: 'invitation/:invitationId', component: InvitationPageComponent, canActivate: [AuthGuard] },
  { path: '', pathMatch: 'full', redirectTo: '/home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
