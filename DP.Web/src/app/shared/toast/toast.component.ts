import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { NotificationService } from '../services/notification.service';
import { NgbToast } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.css'],
  imports: [NgbToast],
})
export class ToastComponent implements OnInit, OnDestroy {
  public readonly notificationService = inject(NotificationService);

  ngOnDestroy(): void {
    this.notificationService.clearToasts();
  }

  ngOnInit(): void {}
}
