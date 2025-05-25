import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface Notification {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  title: string;
  message: string;
  timestamp: Date;
}
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notifications$ = new BehaviorSubject<Notification[]>([]);

  constructor() { }
  getNotifications() {
    return this.notifications$.asObservable();
  }

  showSuccess(title: string, message: string) {
    this.addNotification('success', title, message);
  }

  showError(title: string, message: string) {
    this.addNotification('error', title, message);
  }

  showWarning(title: string, message: string) {
    this.addNotification('warning', title, message);
  }

  showInfo(title: string, message: string) {
    this.addNotification('info', title, message);
  }

  private addNotification(type: Notification['type'], title: string, message: string) {
    const notification: Notification = {
      id: crypto.randomUUID(),
      type,
      title,
      message,
      timestamp: new Date()
    };

    const current = this.notifications$.value;
    this.notifications$.next([notification, ...current]);

    // Auto remove after 5 seconds
    setTimeout(() => {
      this.removeNotification(notification.id);
    }, 5000);
  }

  removeNotification(id: string) {
    const current = this.notifications$.value;
    this.notifications$.next(current.filter(n => n.id !== id));
  }
}
