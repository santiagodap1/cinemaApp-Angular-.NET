import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ApiKeyService } from '../../core/auth/api-key.service';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  constructor(
    private readonly apiKeyService: ApiKeyService,
    private readonly router: Router
  ) {}

  clearAdminKey() {
    this.apiKeyService.clearAdminKey();
    this.router.navigate(['/admin/login']);
  }
}
