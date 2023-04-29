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
  private toasts: ToastInfo[] = [];
  private toastSuccessClassName = 'bg-success text-light';
  private toastErrorClassName = 'bg-danger text-light';
  private defaultDelayTime = 5000;

  public getToasts() {
    return this.toasts;
  }

  public clearToasts() {
    this.toasts = [];
  }

  public showSuccessToast(title: string, message: string, delay = this.defaultDelayTime): void {
    this.toasts.push({title, message, delay, classname: this.toastSuccessClassName});
  }

  public showErrorToast(title: string, message: string, delay = this.defaultDelayTime): void {
    this.toasts.push({title, message, delay, classname: this.toastErrorClassName});
  }

  public showGenericErrorToast(statusCode: number, delay = this.defaultDelayTime) {
    if (statusCode != 0 && statusCode <= 400) {
      return;
    }

    if (statusCode == 0)
      this.toasts.push({
        title: 'Service unavailable.',
        message: 'Please try again later.',
        delay,
        classname: this.toastErrorClassName,
      });
    else if (statusCode >= 400) {
      this.toasts.push({
        title: 'An error occurred.',
        message: 'Please try again later.',
        delay,
        classname: this.toastErrorClassName,
      });
    } else {
      console.warn(`Generic error toast do not support '${statusCode}' status code`);
    }
  }
}
