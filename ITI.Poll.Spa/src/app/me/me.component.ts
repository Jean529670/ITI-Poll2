import { Component, Input } from '@angular/core';
import { Me } from '../poll.service';

@Component({
  selector: 'app-me',
  templateUrl: './me.component.html',
  styleUrls: ['./me.component.css']
})
export class MeComponent {
  @Input() me: Me
}
