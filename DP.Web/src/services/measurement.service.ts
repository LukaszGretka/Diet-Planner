import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, EMPTY, tap } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { Measurement } from 'src/models/measurement';

@Injectable({
  providedIn: 'root'
})
export class MeasurementService {

  private measurementUrl = 'http://localhost:5000/api/measurement';

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient, private router: Router) { }

  getMeasurements(): Observable<Measurement[]> {
    return this.http.get<Measurement[]>(this.measurementUrl);
  }

  getById(id: string): Observable<Measurement> {
    return this.http.get<Measurement>(this.measurementUrl + '/' + id);
  }
}
