import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NicknameValidator } from '../nickname.validator';
import { PollService } from '../poll.service';

@Component({
  selector: 'app-new-poll-page',
  templateUrl: './new-poll-page.component.html',
  styleUrls: ['./new-poll-page.component.css']
})
export class NewPollPageComponent implements OnInit {
  errors: string[]
  newPollForm: FormGroup

  constructor(
    private readonly fb: FormBuilder,
    private readonly pollService: PollService,
    private readonly nicknameValidator: NicknameValidator,
    private readonly router: Router) { }

  ngOnInit(): void {
    this.newPollForm = this.fb.group({
      question: ['', Validators.required],
      proposals: this.fb.array([
        this.fb.control('', Validators.required),
        this.fb.control('', Validators.required)
      ]),
      guests: this.fb.array([
        this.fb.control('', [Validators.required], [this.nicknameValidator.validate.bind(this.nicknameValidator)])
      ])
    });
  }

  onSubmit() {
    this.pollService.createPoll({
      question: this.question.value,
      proposals: this.proposals.value,
      guests: this.guests.value
    }).subscribe(pollResult => {
      if (typeof(pollResult.errors) != 'undefined' && pollResult.errors.length > 0) {
        this.errors = ['Unable to create poll'];
      } else if (pollResult.data.poll.createPoll.errors.length > 0) {
        this.errors = pollResult.data.poll.createPoll.errors.map(e => e.message);
      } else {
        this.router.navigate(['poll', pollResult.data.poll.createPoll.poll.pollId]);
      }
    });
  }

  addProposal() {
    this.proposals.push(this.fb.control('', Validators.required));
  }

  deleteProposal(i: number) {
    this.proposals.removeAt(i);
  }

  addGuest() {
    this.guests.push(this.fb.control('', [Validators.required], [this.nicknameValidator.validate.bind(this.nicknameValidator)]));
  }

  deleteGuest(i: number) {
    this.guests.removeAt(i);
  }

  get question() { return this.newPollForm.get('question'); }

  get proposals() { return this.newPollForm.get('proposals') as FormArray; }

  get guests() { return this.newPollForm.get('guests') as FormArray; }
}
