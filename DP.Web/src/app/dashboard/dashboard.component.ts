import {Component, OnInit} from '@angular/core';
import * as GeneralSelector from '../stores/store.selectors';
import {Store} from '@ngrx/store';
import {GeneralState} from '../stores/store.state';
import {AccountState} from '../account/stores/account.state';
import * as AccountSelector from '../account/stores/account.selector';
import {AccountService} from '../account/services/account.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);
  public authenticatedUser$ = this.accountService.getUser();

  constructor(
    private store: Store<GeneralState>,
    private accountStore: Store<AccountState>,
    private accountService: AccountService,
  ) {}

  ngOnInit(): void {}
}
