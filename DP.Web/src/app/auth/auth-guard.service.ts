import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../account/services/account.service';
import { UntilDestroy } from '@ngneat/until-destroy';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
@UntilDestroy()
export class AuthGuardService  {
  constructor(public accountService: AccountService, public router: Router) {}

  canActivate(): Observable<boolean> {
    return this.accountService.isAuthenticated().pipe(
      map(isAuthenticated => {
        if (!isAuthenticated) {
          this.router.navigate(['/unauthorized']);
          return false;
        }
        return true;
      }),
    );
  }
}
