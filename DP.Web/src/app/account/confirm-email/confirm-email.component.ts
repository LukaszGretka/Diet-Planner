import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountState } from '../stores/account.state';
import { Store } from '@ngrx/store';
import * as AccountActions from '../stores/account.actions';
import { take } from 'rxjs';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css'],
})
export class ConfirmEmailComponent implements OnInit {
  constructor(private route: ActivatedRoute, private accountStore: Store<AccountState>, private router: Router) {}

  private email: string;
  private confirmationToken: string;

  ngOnInit(): void {
    this.route.queryParams.pipe(take(1)).subscribe(params => {
      this.confirmationToken = params['confirmationToken'];
      this.email = params['email'];
      if (!this.confirmationToken || !this.email) {
        this.router.navigate['sign-in'];
        return;
      }
    });
  }
  public onEmailConfirmationButtonClick() {
    this.accountStore.dispatch(
      AccountActions.confirmEmailRequest({
        emailConfirmationRequest: {
          email: this.email,
          confirmationToken: this.confirmationToken,
        },
      }),
    );
  }
}
