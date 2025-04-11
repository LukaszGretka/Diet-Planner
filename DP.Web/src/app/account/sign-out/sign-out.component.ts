import { Component, OnInit, inject } from '@angular/core';
import * as AccountActions from '../stores/account.actions';
import { AccountState } from '../stores/account.state';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-sign-out',
  templateUrl: './sign-out.component.html',
  styleUrls: ['./sign-out.component.css'],
  standalone: false,
})
export class SignOutComponent implements OnInit {
  private readonly accountStore = inject<Store<AccountState>>(Store);

  public ngOnInit(): void {
    this.accountStore.dispatch(AccountActions.signOutRequest());
  }
}
