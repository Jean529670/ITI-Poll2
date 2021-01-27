import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, mergeMap } from 'rxjs/operators';
import { Invitation, PollService } from '../poll.service';

@Component({
  selector: 'app-invitation-page',
  templateUrl: './invitation-page.component.html',
  styleUrls: ['./invitation-page.component.css']
})
export class InvitationPageComponent implements OnInit {

  invitation$: Observable<Invitation>

  constructor(
    private readonly pollService: PollService,
    private readonly route: ActivatedRoute,
    private readonly router: Router) { }

  ngOnInit(): void {
    this.invitation$ = this.route.paramMap
      .pipe(
        map(p => Number.parseInt(p.get('invitationId'))),
        mergeMap(invitationId => this.pollService.getInvitationById(invitationId)),
        map(result => result.data.invitation.invitation)
      );
  }

  onVoted() {
    this.router.navigate(['home']);
  }

}
