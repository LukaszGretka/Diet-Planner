import { Component, OnDestroy, OnInit } from '@angular/core';
import { NotificationService } from '../services/notification.service';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.css'],
})
export class ToastComponent implements OnInit, OnDestroy {
  constructor(public notificationService: NotificationService) {}

  ngOnDestroy(): void {
    this.notificationService.clearToasts();
  }

  ngOnInit(): void {}
}
