import { Injectable } from '@angular/core';
import { ApiClient } from '../../core/api/api-client.service';
import { CreateSeatRequest, Seat } from './seats.models';

@Injectable({ providedIn: 'root' })
export class SeatsService {
  constructor(private readonly apiClient: ApiClient) {}

  getByAuditorium(auditoriumId: string, page: number, pageSize: number) {
    return this.apiClient.get<Seat[]>(`/api/auditoriums/${auditoriumId}/seats`, { page, pageSize });
  }

  create(payload: CreateSeatRequest) {
    return this.apiClient.post<Seat>(`/api/auditoriums/${payload.auditoriumId}/seats`, payload);
  }

  delete(auditoriumId: string, seatId: string) {
    return this.apiClient.delete<void>(`/api/auditoriums/${auditoriumId}/seats/${seatId}`);
  }
}
