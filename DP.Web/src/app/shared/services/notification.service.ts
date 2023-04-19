import {Injectable} from '@angular/core';

export interface ToastInfo {
  title: string;
  message: string;
  delay?: number;
  classname: any;
}

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  toasts: ToastInfo[] = [];

  public showSuccessToast(title: string, message: string, delay: number = 5000): void {
    this.toasts.push({title, message, delay, classname: 'bg-success text-light'});
  }

  public showErrorToast(title: string, message: string, delay: number = 5000): void {
    this.toasts.push({title, message, delay, classname: 'bg-danger text-light'});
  }
}
