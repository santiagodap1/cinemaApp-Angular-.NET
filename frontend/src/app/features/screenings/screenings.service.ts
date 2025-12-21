import { Injectable } from '@angular/core';
import { ApiClient } from '../../core/api/api-client.service';
import { CreateScreeningRequest, Screening, ScreeningOccupancy } from './screenings.models';

@Injectable({ providedIn: 'root' })
export class ScreeningsService {
  constructor(private readonly apiClient: ApiClient) {}

  getAll(filters: { movieId?: string; cinemaSiteId?: string; date?: string }, page: number, pageSize: number) {
    return this.apiClient.get<Screening[]>('/api/screenings', {
      movieId: filters.movieId || undefined,
      cinemaSiteId: filters.cinemaSiteId || undefined,
      date: filters.date || undefined,
      page,
      pageSize
    });
  }

  getOccupancy(screeningId: string) {
    return this.apiClient.get<ScreeningOccupancy>(`/api/screenings/${screeningId}/occupancy`);
  }

  create(payload: CreateScreeningRequest) {
    return this.apiClient.post<Screening>('/api/screenings', payload);
  }
}
