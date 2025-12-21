import { Injectable } from '@angular/core';

const USER_KEY_STORAGE = 'cinema.userKey';
const ADMIN_KEY_STORAGE = 'cinema.adminKey';

@Injectable({ providedIn: 'root' })
export class ApiKeyService {
  getUserKey(): string {
    return localStorage.getItem(USER_KEY_STORAGE) ?? '';
  }

  setUserKey(key: string) {
    localStorage.setItem(USER_KEY_STORAGE, key.trim());
  }

  clearUserKey() {
    localStorage.removeItem(USER_KEY_STORAGE);
  }

  getAdminKey(): string {
    return localStorage.getItem(ADMIN_KEY_STORAGE) ?? '';
  }

  setAdminKey(key: string) {
    localStorage.setItem(ADMIN_KEY_STORAGE, key.trim());
  }

  clearAdminKey() {
    localStorage.removeItem(ADMIN_KEY_STORAGE);
  }

  resolveKey(path: string): string {
    if (path.startsWith('/api/admin')) {
      return this.getAdminKey() || this.getUserKey();
    }

    return this.getUserKey();
  }
}
