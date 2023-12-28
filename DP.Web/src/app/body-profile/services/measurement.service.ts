import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Measurement } from 'src/app/body-profile/models/measurement';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MeasurementService {
  private measurementUrl = `${environment.dietPlannerApiUri}/api/measurement`;

  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient) {}

  getMeasurements(): Observable<Measurement[]> {
    return this.httpClient.get<Measurement[]>(this.measurementUrl, { withCredentials: true });
  }

  getById(id: number): Observable<Measurement> {
    return this.httpClient.get<Measurement>(this.measurementUrl + '/' + id, {
      withCredentials: true,
    });
  }

  addMeasurement(measurementData: Measurement): Observable<Measurement> {
    return this.httpClient.post<Measurement>(this.measurementUrl, measurementData, this.httpOptions);
  }

  editMeasurement(measurementId: number, measurementData: Measurement): Observable<Measurement> {
    return this.httpClient.put<Measurement>(
      this.measurementUrl + '/' + measurementId,
      measurementData,
      this.httpOptions,
    );
  }

  deleteMeasurement(measurementId: number): Observable<Measurement> {
    return this.httpClient.delete<Measurement>(this.measurementUrl + '/' + measurementId, this.httpOptions);
  }
}
