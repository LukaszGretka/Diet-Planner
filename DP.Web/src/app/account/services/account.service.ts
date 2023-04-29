import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BehaviorSubject, Observable, catchError, map, of} from 'rxjs';
import {SignInRequest} from '../models/sign-in-request';
import {SignInResult} from '../models/sign-in-result';
import {SignUpRequest} from '../models/sign-up-request';
import {User} from '../models/user';

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

  constructor(private httpClient: HttpClient) {}

  public performSignUp(signupRequest: SignUpRequest): Observable<User> {
    return this.httpClient.post<User>(this.baseUrl + '/sign-up', signupRequest, this.httpOptions);
  }

  public performSignIn(signInRequest: SignInRequest): Observable<SignInResult> {
    return this.httpClient.post<SignInResult>(this.baseUrl + '/sign-in', signInRequest, this.httpOptions);
  }

  public getUserClaims(): Observable<User> {
    return this.httpClient.get<User>(this.baseUrl, {withCredentials: true});
  }

  public performSignOut() {
    return this.httpClient.post<any>(this.baseUrl + '/signout', null, this.httpOptions);
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
}
