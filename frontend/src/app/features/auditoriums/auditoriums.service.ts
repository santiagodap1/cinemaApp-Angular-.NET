import { Injectable } from '@angular/core';
import { ApiClient } from '../../core/api/api-client.service';
import { Auditorium, CreateAuditoriumRequest, UpdateAuditoriumRequest } from './auditoriums.models';

@Injectable({ providedIn: 'root' })
export class AuditoriumsService {
  constructor(private readonly apiClient: ApiClient) {}

  getAll(cinemaSiteId: string | null, page: number, pageSize: number) {
    return this.apiClient.get<Auditorium[]>('/api/auditoriums', {
      cinemaSiteId: cinemaSiteId || undefined,
      page,
      pageSize
    });
  }

  create(payload: CreateAuditoriumRequest) {
    return this.apiClient.post<Auditorium>('/api/auditoriums', payload);
  }

  update(id: string, payload: UpdateAuditoriumRequest) {
    return this.apiClient.put<Auditorium>(`/api/auditoriums/${id}`, payload);
  }

  delete(id: string) {
    return this.apiClient.delete<void>(`/api/auditoriums/${id}`);
  }
}
