import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { EMPTY, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AccountService } from '../account/services/account.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router, private accountService: AccountService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        // TODO: Just for now. Find better way to handle it.
        if (request.url.startsWith('http://localhost:5000/') && error.status === 401
          && request.url != 'http://localhost:5000/api/account') {
          this.accountService.authenticatedUser$.next(null);
          this.router.navigate(['/unauthorized']);
        }
        return EMPTY;
      })
    );
  }
}
