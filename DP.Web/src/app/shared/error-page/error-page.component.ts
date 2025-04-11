import { HttpStatusCode } from '@angular/common/http';
import { Component, Input, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-error-page',
    templateUrl: './error-page.component.html',
    styleUrls: ['./error-page.component.css']
})
export class ErrorPageComponent {
  private router = inject(Router);

  @Input()
  public httpsStatusError: HttpStatusCode;

  constructor() {
    if (this.httpsStatusError === undefined || this.httpsStatusError === null) {
      console.debug('No error code provided. Set default http status code: 500');
    }
    this.httpsStatusError = 500;
  }

  public onReturnToDashboardClick() {
    this.router.navigateByUrl('/dashboard');
  }
}
