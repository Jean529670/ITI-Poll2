import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map, mergeMap } from 'rxjs/operators';
import { Poll, PollService } from '../poll.service';

@Component({
  selector: 'app-poll-page',
  templateUrl: './poll-page.component.html',
  styleUrls: ['./poll-page.component.css']
})
export class PollPageComponent implements OnInit {
  poll$: Observable<Poll>
  
  constructor(
    private readonly route: ActivatedRoute,
    private readonly pollService: PollService) { }

  ngOnInit(): void {
    this.poll$ = this.route.paramMap
      .pipe(
        map(p => Number.parseInt(p.get('pollId'))),
        mergeMap(pollId => this.pollService.getPollById(pollId)),
        map(r => r.data.poll.poll)
      );
  }

}
