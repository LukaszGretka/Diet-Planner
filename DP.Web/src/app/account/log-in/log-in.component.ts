import { Component } from "@angular/core";
import { Store } from "@ngrx/store";
import { LogInRequest } from "../models/log-in-request";
import * as  AccountActions from "../stores/account.actions";
import * as AccountSelector from "../stores/account.selector";

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent {

  public user$ = this.store.select(AccountSelector.getUser);

  constructor(
    private store: Store
  ) { }


  public onLogInSubmit(logInRequest: LogInRequest): void {
    this.store.dispatch(AccountActions.logInRequest({ logInRequest }));
  }
}
