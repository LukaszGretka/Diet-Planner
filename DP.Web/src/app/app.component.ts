import { Component } from '@angular/core';
import { AccountService } from './account/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  constructor(private accountService: AccountService) {
    if (accountService.isAuthenticated()) {
      this.accountService.getUserClaims()
        .subscribe(user => this.accountService.authenticatedUser$.next(user));
    }
  }
}
