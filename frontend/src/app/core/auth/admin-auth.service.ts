import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AdminAuthService {
  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private readonly http: HttpClient) {}

  validateAdminKey(key: string) {
    const headers = new HttpHeaders({
      'X-Api-Key': key.trim()
    });

    return this.http.get<{ status: string }>(`${this.baseUrl}/api/admin/validate`, { headers });
  }
}
