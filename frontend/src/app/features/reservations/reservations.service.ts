import { Injectable } from '@angular/core';
import { ApiClient } from '../../core/api/api-client.service';
import { CreateReservationRequest, ReservationResponse } from './reservations.models';

@Injectable({ providedIn: 'root' })
export class ReservationsService {
  constructor(private readonly apiClient: ApiClient) {}

  create(payload: CreateReservationRequest) {
    return this.apiClient.post<ReservationResponse>('/api/reservations', payload);
  }

  confirm(reservationId: string) {
    return this.apiClient.post<ReservationResponse>(`/api/reservations/${reservationId}/confirm`, {});
  }

  cancel(reservationId: string) {
    return this.apiClient.post<ReservationResponse>(`/api/reservations/${reservationId}/cancel`, {});
  }
}
