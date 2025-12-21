import { fakeAsync, tick } from '@angular/core/testing';
import { NotificationService } from './notification.service';

describe('NotificationService', () => {
  it('adds and dismisses messages', fakeAsync(() => {
    const service = new NotificationService();
    let latest: unknown[] = [];

    service.messages$.subscribe((messages) => {
      latest = messages;
    });

    service.show('Hello', 'success');
    expect(latest.length).toBe(1);

    tick(4000);
    expect(latest.length).toBe(0);
  }));
});
