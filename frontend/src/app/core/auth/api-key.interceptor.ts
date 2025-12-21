import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { ApiKeyService } from './api-key.service';

export const apiKeyInterceptor: HttpInterceptorFn = (req, next) => {
  const apiKeyService = inject(ApiKeyService);
  const apiKey = apiKeyService.resolveKey(req.url);

  if (!apiKey) {
    return next(req);
  }

  return next(req.clone({
    setHeaders: {
      'X-Api-Key': apiKey
    }
  }));
};
