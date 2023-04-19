import {Component, OnInit} from '@angular/core';
import * as GeneralSelector from '../stores/store.selectors';
import {Store} from '@ngrx/store';
import {GeneralState} from '../stores/store.state';
import {AccountState} from '../account/stores/account.state';
import * as AccountSelector from '../account/stores/account.selector';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);
  public authenticatedUser$ = this.accountStore.select(AccountSelector.getAuthenticatedUser);

  constructor(private store: Store<GeneralState>, private accountStore: Store<AccountState>) {}

  ngOnInit(): void {}
}
