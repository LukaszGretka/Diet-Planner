import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, EMPTY, tap } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { Measurement } from 'src/models/measurement';

@Injectable({
  providedIn: 'root'
})
export class MeasurementService {

  private measurementUrl = 'http://localhost:5000/api/measurement';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getMeasurements(): Observable<Measurement[]> {
    return this.http.get<Measurement[]>(this.measurementUrl);
  }

  addMeasurement(measurement: Measurement): Observable<Measurement> {
    return this.http.post<Measurement>(this.measurementUrl, measurement, this.httpOptions).pipe(
      tap((newMeasurement: Measurement) => console.log(`added new measurement: ${newMeasurement}`)),
      catchError((error) => {console.log('error: ' + error.message); return EMPTY})
    );
  }
}
