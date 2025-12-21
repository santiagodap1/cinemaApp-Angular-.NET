import { Component } from '@angular/core';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  template: `
    <section class="panel">
      <h1>Admin Dashboard</h1>
      <p>Manage system utilities and seed data.</p>
    </section>
  `,
  styles: []
})
export class AdminDashboardComponent {}
