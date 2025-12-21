import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type NotificationType = 'success' | 'error' | 'info';

export interface NotificationMessage {
  id: string;
  type: NotificationType;
  message: string;
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly messagesSubject = new BehaviorSubject<NotificationMessage[]>([]);
  readonly messages$ = this.messagesSubject.asObservable();

  show(message: string, type: NotificationType = 'info') {
    const id = crypto.randomUUID();
    const next = [...this.messagesSubject.value, { id, type, message }];
    this.messagesSubject.next(next);

    setTimeout(() => this.dismiss(id), 4000);
  }

  dismiss(id: string) {
    this.messagesSubject.next(this.messagesSubject.value.filter((item) => item.id !== id));
  }
}
