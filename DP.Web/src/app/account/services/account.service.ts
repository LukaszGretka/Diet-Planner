import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {LogInRequest} from '../models/log-in-request';
import {LogInResult} from '../models/log-in-result';
import {SignUpRequest} from '../models/sign-up-request';
import {User} from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  public user$: Observable<User>;

  private baseUrl = 'http://localhost:5000/api/account';

  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient) {}

  public performSignUp(signupRequest: SignUpRequest): Observable<User> {
    this.user$ = this.httpClient.post<User>(this.baseUrl + '/signup', signupRequest, this.httpOptions);
    return this.user$;
  }

  public performLogIn(loginRequest: LogInRequest): Observable<User> {
    this.user$ = this.httpClient.post<User>(this.baseUrl + '/login', loginRequest, this.httpOptions);
    return this.user$;
  }

  public getUserClaims(): Observable<User> {
    return this.httpClient.get<User>(this.baseUrl, {withCredentials: true});
  }
}
