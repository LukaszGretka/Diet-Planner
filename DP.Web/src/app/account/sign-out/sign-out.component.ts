import { Component, OnInit } from '@angular/core';
import * as AccountActions from '../stores/account.actions';
import { AccountState } from '../stores/account.state';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-sign-out',
  templateUrl: './sign-out.component.html',
  styleUrls: ['./sign-out.component.css'],
})
export class SignOutComponent implements OnInit {
  constructor(private accountStore: Store<AccountState>) {}

  public ngOnInit(): void {
    this.accountStore.dispatch(AccountActions.signOutRequest());
  }
}
