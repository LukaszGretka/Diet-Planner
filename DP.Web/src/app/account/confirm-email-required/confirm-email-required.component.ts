import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-confirm-email-required',
  templateUrl: './confirm-email-required.component.html',
  styleUrls: ['./confirm-email-required.component.css'],
  imports: [RouterLink],
})
export class ConfirmEmailRequiredComponent implements OnInit {
  constructor() {}

  ngOnInit(): void {}
}
