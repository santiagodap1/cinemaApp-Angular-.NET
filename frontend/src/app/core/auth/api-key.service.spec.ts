import { ApiKeyService } from './api-key.service';

describe('ApiKeyService', () => {
  let service: ApiKeyService;

  beforeEach(() => {
    localStorage.clear();
    service = new ApiKeyService();
  });

  it('stores and resolves admin key for admin paths', () => {
    service.setAdminKey('admin-key');

    expect(service.resolveKey('/api/admin/seed')).toBe('admin-key');
  });

  it('falls back to user key for non-admin paths', () => {
    service.setUserKey('user-key');

    expect(service.resolveKey('/api/movies')).toBe('user-key');
  });
});
