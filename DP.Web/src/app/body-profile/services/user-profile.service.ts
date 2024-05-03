import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserProfile } from '../models/user-profile';
import { Observable } from 'rxjs';
import { UserAvatar } from '../models/user-avatar';

@Injectable({
  providedIn: 'root',
})
export class UserProfileService {
  private userProfileApiUrl = `${environment.dietPlannerApiUri}/api/userprofile`;

  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient) { }

  public getUserProfile(): Observable<UserProfile> {
    return this.httpClient.get<UserProfile>(this.userProfileApiUrl, { withCredentials: true });
  }

  public updateUserProfile(userProfile: UserProfile): Observable<UserProfile> {
    return this.httpClient.patch<UserProfile>(this.userProfileApiUrl, userProfile, this.httpOptions);
  }

  public updateUserAvatar(content: string): Observable<UserProfile> {
    return this.httpClient.patch<UserProfile>(`${this.userProfileApiUrl}/avatar`, { base64Image: content }, this.httpOptions);
  }
}
