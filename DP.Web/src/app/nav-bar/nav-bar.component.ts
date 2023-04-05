import {Component, OnInit} from '@angular/core';
import {AccountService} from '../account/services/account.service';
import {Observable} from 'rxjs';
import {User} from '../account/models/user';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css'],
})
export class NavBarComponent implements OnInit {
  public user$: Observable<User>;

  constructor(private accountService: AccountService) {
    this.user$ = this.accountService.user$;
  }

  ngOnInit(): void {}
}
