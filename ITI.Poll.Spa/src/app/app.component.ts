import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AppState } from './app.state';
import { selectIsAuthenticated, selectCurrentEmail } from './identity.selectors';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  isAuthenticated$: Observable<boolean>
  email$: Observable<string>
  
  constructor(private readonly store: Store<AppState>) { }

  ngOnInit(): void {
    this.isAuthenticated$ = this.store.select(selectIsAuthenticated);
    this.email$ = this.store.select(selectCurrentEmail);
  }

}
