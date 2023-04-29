import { Component, OnDestroy, OnInit } from '@angular/core';
import { NotificationService } from '../services/notification.service';

@Component({
  selector: 'app-toaster',
  templateUrl: './toaster.component.html',
  styleUrls: ['./toaster.component.css'],
})
export class ToasterComponent implements OnInit, OnDestroy {
  constructor(public notificationService: NotificationService) {}

  ngOnDestroy(): void {
    this.notificationService.clearToasts();
  }

  ngOnInit(): void {}
}
