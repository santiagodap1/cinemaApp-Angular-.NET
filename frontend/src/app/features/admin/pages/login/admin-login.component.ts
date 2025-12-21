import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiKeyService } from '../../../../core/auth/api-key.service';
import { AdminAuthService } from '../../../../core/auth/admin-auth.service';

@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-login.component.html',
  styleUrl: './admin-login.component.scss'
})
export class AdminLoginComponent {
  adminKey = '';
  error = '';
  isLoading = false;

  constructor(
    private readonly apiKeyService: ApiKeyService,
    private readonly adminAuth: AdminAuthService,
    private readonly router: Router
  ) {}

  submit() {
    const trimmedKey = this.adminKey.trim();
    if (!trimmedKey) {
      this.error = 'Admin key is required.';
      return;
    }

    this.isLoading = true;
    this.error = '';

    this.adminAuth.validateAdminKey(trimmedKey).subscribe({
      next: () => {
        this.apiKeyService.setAdminKey(trimmedKey);
        this.router.navigate(['/admin']);
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Invalid admin key.';
        this.isLoading = false;
      }
    });
  }
}
