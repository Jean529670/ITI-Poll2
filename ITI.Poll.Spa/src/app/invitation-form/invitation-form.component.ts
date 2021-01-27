import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Invitation, PollService } from '../poll.service';

@Component({
  selector: 'app-invitation-form',
  templateUrl: './invitation-form.component.html',
  styleUrls: ['./invitation-form.component.css']
})
export class InvitationFormComponent {

  @Input() invitation: Invitation;

  @Output() voted = new EventEmitter<boolean>();

  invitationForm = this.fb.group({
    proposalId: [0, Validators.min(16)]
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly pollService: PollService) { }

  onSubmit() {
    this.pollService.vote(
      this.invitation.invitationId,
      this.proposal.value)
      .subscribe(() => this.voted.emit(true));
  }

  get proposal() { return this.invitationForm.get('proposalId'); }

}
