import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
	constructor(private router: Router) {}

	intercept(request: HttpRequest<any>, next: HttpHandler) {
		return next.handle(request).pipe(
			catchError((error: HttpErrorResponse) => {
				//TODO: remove hardcoded localhost:5000 (api host)
				if (request.url.startsWith('http://localhost:5000/') && error.status === 401) {
					this.router.navigate(['/unauthorized']);
				}
				return throwError(() => 'unauthorized');
			})
		);
	}
}
