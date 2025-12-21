import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { ApiKeyService } from './api-key.service';

export const adminKeyGuard: CanActivateFn = () => {
  const apiKeyService = inject(ApiKeyService);
  const router = inject(Router);

  if (apiKeyService.getAdminKey()) {
    return true;
  }

  router.navigate(['/admin/login']);
  return false;
};
