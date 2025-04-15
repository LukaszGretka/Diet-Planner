import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Observable, catchError, map, of } from 'rxjs';
import { SignInRequest } from '../models/sign-in-request';
import { SignInResult } from '../models/sign-in-result';
import { SignUpRequest } from '../models/sign-up-request';
import { User } from '../models/user';
import { EmailConfirmationRequest } from '../models/email-confirmation-request';
import { SignUpResult } from '../models/sign-up-result';
import { environment } from 'src/environments/environment';
import { ChangePasswordRequest } from '../models/change-password-request';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private readonly httpClient = inject(HttpClient);

  public authenticatedUser$ = new BehaviorSubject<User>(null);

  private baseUrl = `${environment.dietPlannerApiUri}/api/account`;

  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  public performSignUp(signupRequest: SignUpRequest): Observable<SignUpResult> {
    return this.httpClient.post<SignUpResult>(this.baseUrl + '/sign-up', signupRequest, this.httpOptions);
  }

  public performSignIn(signInRequest: SignInRequest): Observable<SignInResult> {
    return this.httpClient.post<SignInResult>(this.baseUrl + '/sign-in', signInRequest, this.httpOptions);
  }

  public getUserClaims(): Observable<User> {
    return this.httpClient.get<User>(this.baseUrl, { withCredentials: true });
  }

  public performSignOut() {
    return this.httpClient.post<any>(this.baseUrl + '/signout', null, this.httpOptions);
  }

  public performConfirmEmail(emailConformationRequest: EmailConfirmationRequest): Observable<any> {
    return this.httpClient.post<EmailConfirmationRequest>(
      this.baseUrl + '/confirm-email',
      emailConformationRequest,
      this.httpOptions,
    );
  }

  public setUser(user: User) {
    this.authenticatedUser$.next(user);
  }

  public getUser(): Observable<User> {
    return this.authenticatedUser$.asObservable();
  }

  public isAuthenticated(): Observable<boolean> {
    if (this.authenticatedUser$.getValue()) {
      return of(true);
    }

    return this.getUserClaims().pipe(
      map(user => {
        this.setUser(user);
        return user !== null;
      }),
      catchError(() => of(false)),
    );
  }

  public changePassword(changePasswordRequest: ChangePasswordRequest): Observable<any> {
    return this.httpClient.post<any>(this.baseUrl + '/change-password', changePasswordRequest, this.httpOptions);
  }
}
