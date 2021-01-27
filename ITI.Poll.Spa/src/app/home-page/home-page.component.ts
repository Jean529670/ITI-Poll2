import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Me, PollService } from '../poll.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  me$: Observable<Me>;

  constructor(
    private readonly pollService: PollService,
    private readonly router: Router) { }

  ngOnInit(): void {
    this.me$ = this.pollService.getMyPolls().pipe(map(x => x.data.me));
  }

  newPoll() {
    this.router.navigate(['new-poll']);
  }

}
