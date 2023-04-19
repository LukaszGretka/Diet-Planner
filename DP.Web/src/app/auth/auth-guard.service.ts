import {Injectable} from '@angular/core';
import {Router, CanActivate} from '@angular/router';
import {AccountService} from '../account/services/account.service';
import {UntilDestroy, untilDestroyed} from '@ngneat/until-destroy';

@Injectable()
@UntilDestroy()
export class AuthGuardService implements CanActivate {
  private userSignedIn = false;

  constructor(public accountService: AccountService, public router: Router) {
    this.accountService
      .isAuthenticated()
      .pipe(untilDestroyed(this))
      .subscribe(authenticatedUser => {
        this.userSignedIn = authenticatedUser;
      });
  }
  canActivate(): boolean {
    if (!this.userSignedIn) {
      this.router.navigate(['unauthorized']);
      return false;
    }
    return true;
  }
}
