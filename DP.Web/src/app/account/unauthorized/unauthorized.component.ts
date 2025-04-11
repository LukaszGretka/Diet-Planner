import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.css'],
})
export class UnauthorizedComponent implements OnInit {
  private readonly router = inject(Router);

  ngOnInit(): void {}

  public onGoToSignInClick() {
    this.router.navigateByUrl('/sign-in');
  }
}
