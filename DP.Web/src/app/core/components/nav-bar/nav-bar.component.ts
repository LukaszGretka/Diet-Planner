import { Component, OnInit, inject } from '@angular/core';
import { AccountService } from '../../../account/services/account.service';
import { Observable } from 'rxjs';
import { User } from '../../../account/models/user';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AsyncPipe } from '@angular/common';

@UntilDestroy()
@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css'],
  imports: [RouterLink, RouterLinkActive, AsyncPipe],
})
export class NavBarComponent implements OnInit {
  private accountService = inject(AccountService);
  public user$: Observable<User>;

  constructor() {
    this.user$ = this.accountService.authenticatedUser$;
  }

  ngOnInit(): void {
    this.user$.pipe(untilDestroyed(this)).subscribe(user => {
      this.setNavigationLinks(user);
    });
  }

  private setNavigationLinks(user: User): void {
    const links = document.getElementsByClassName('nav-link');
    if (!links) {
      return;
    }

    for (let index = 0; index < links.length; index++) {
      // never disable sign-in navigation link
      if (links[index].classList.contains('sign-in-link')) {
        continue;
      }
      if (!user) {
        links[index].classList.add('disabled');
      } else if (links[index].classList.contains('disabled')) {
        links[index].classList.remove('disabled');
      }
    }
  }
}
