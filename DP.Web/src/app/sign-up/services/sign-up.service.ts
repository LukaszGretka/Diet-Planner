import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SignUpRequestData } from "../models/sign-up-request-data";

@Injectable({
  providedIn: 'root'
})
export class SignUpService {
  private signUpUrl = 'http://localhost:5000/api/signup';

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private httpClient: HttpClient) { }

  submitSignUp(signUpData: SignUpRequestData): Observable<any> {
    return this.httpClient.post<SignUpRequestData>(this.signUpUrl, signUpData, this.httpOptions);
  }

}
