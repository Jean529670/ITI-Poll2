import { Component, Input } from '@angular/core';
import { Poll } from '../poll.service';

@Component({
  selector: 'app-poll',
  templateUrl: './poll.component.html',
  styleUrls: ['./poll.component.css']
})
export class PollComponent {
  @Input() poll: Poll

  answers() {
    return this.poll.answers
      .map(a => a.votesCount)
      .reduce((x, y) => x + y);
  }
}
