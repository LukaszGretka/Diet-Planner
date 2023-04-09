import { Component, OnInit } from '@angular/core';
import * as GeneralSelector from '../stores/store.selectors';
import { Store } from '@ngrx/store';
import { GeneralState } from '../stores/store.state';
import { AccountState } from '../account/stores/account.state';
import * as AccountSelector from '../account/stores/account.selector';
import { AccountService } from '../account/services/account.service';
import { Observable } from 'rxjs';
import { User } from '../account/models/user';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);
  public user$: Observable<User>;

  constructor(private store: Store<GeneralState>, private accountStore: Store<AccountState>,
    private authService: AccountService) {
    console.log(this.authService.authenticatedUser$.getValue());
    this.user$ = this.authService.authenticatedUser$.asObservable();
  }

  ngOnInit(): void { }
}
