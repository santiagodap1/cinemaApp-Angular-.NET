import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiClient } from '../../../../core/api/api-client.service';
import { NotificationService } from '../../../../core/notifications/notification.service';

@Component({
  selector: 'app-admin-seed',
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="panel stack">
      <h1>Seed Database</h1>
      <p>This will populate the database with demo data.</p>
      <button class="btn btn--primary" (click)="seed()" [disabled]="isLoading">Seed Now</button>
      <p class="status" *ngIf="status">{{ status }}</p>
    </section>
  `,
  styles: []
})
export class AdminSeedComponent {
  status = '';
  isLoading = false;

  constructor(
    private readonly apiClient: ApiClient,
    private readonly notifications: NotificationService
  ) {}

  seed() {
    this.isLoading = true;
    this.status = '';

    this.apiClient.post<{ status: string }>('api/admin/seed', {}).subscribe({
      next: (response) => {
        this.status = response.status ?? 'Seed completed.';
        this.notifications.show('Seed completed.', 'success');
        this.isLoading = false;
      },
      error: () => {
        this.status = 'Seed failed.';
        this.notifications.show('Seed failed.', 'error');
        this.isLoading = false;
      }
    });
  }
}
