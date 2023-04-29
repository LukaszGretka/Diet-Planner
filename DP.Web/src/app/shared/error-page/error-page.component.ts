import { HttpStatusCode } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.css'],
})
export class ErrorPageComponent {
  @Input()
  public httpsStatusError: HttpStatusCode;

  constructor(private router: Router) {
    if (this.httpsStatusError === undefined || this.httpsStatusError === null) {
      console.debug('No error code provided. Set default http status code: 500');
    }
    this.httpsStatusError = 500;
  }

  public onReturnToDashboardClick() {
    this.router.navigateByUrl('/dashboard');
  }
}
