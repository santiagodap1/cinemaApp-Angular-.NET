import { Injectable } from '@angular/core';
import { ApiClient } from '../../core/api/api-client.service';
import { CinemaSite, CreateCinemaSiteRequest, UpdateCinemaSiteRequest } from './cinema-sites.models';

@Injectable({ providedIn: 'root' })
export class CinemaSitesService {
  constructor(private readonly apiClient: ApiClient) {}

  getAll(page: number, pageSize: number) {
    return this.apiClient.get<CinemaSite[]>('/api/cinema-sites', { page, pageSize });
  }

  create(payload: CreateCinemaSiteRequest) {
    return this.apiClient.post<CinemaSite>('/api/cinema-sites', payload);
  }

  update(id: string, payload: UpdateCinemaSiteRequest) {
    return this.apiClient.put<CinemaSite>(`/api/cinema-sites/${id}`, payload);
  }

  delete(id: string) {
    return this.apiClient.delete<void>(`/api/cinema-sites/${id}`);
  }
}
