import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map, tap } from 'rxjs';
import { LogInRequest } from '../models/log-in-request';
import { LogInResult } from '../models/log-in-result';
import { SignUpRequest } from '../models/sign-up-request';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  public authenticatedUser$ = new BehaviorSubject<User>(null);

  private baseUrl = 'http://localhost:5000/api/account';

  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient) { }

  public performSignUp(signupRequest: SignUpRequest): Observable<User> {
    return this.httpClient.post<User>(this.baseUrl + '/signup', signupRequest, this.httpOptions);
  }

  public performLogIn(loginRequest: LogInRequest): Observable<LogInResult> {
    return this.httpClient.post<LogInResult>(this.baseUrl + '/login', loginRequest, this.httpOptions);
  }

  public getUserClaims(): Observable<User> {
    return this.httpClient.get<User>(this.baseUrl, { withCredentials: true });
  }

  public performSignOut() {
    return this.httpClient.post<any>(this.baseUrl + '/signout', null, this.httpOptions);
  }

  public isAuthenticated(): Observable<boolean> {
    return this.authenticatedUser$.pipe(map(user => user != null));
  }
}
