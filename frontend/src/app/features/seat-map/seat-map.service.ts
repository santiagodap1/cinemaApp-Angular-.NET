import { Injectable } from '@angular/core';
import { ApiClient } from '../../core/api/api-client.service';
import { SeatMapResponse } from './seat-map.models';

@Injectable({ providedIn: 'root' })
export class SeatMapService {
  constructor(private readonly apiClient: ApiClient) {}

  getByScreening(screeningId: string) {
    return this.apiClient.get<SeatMapResponse>(`/api/screenings/${screeningId}/seats`);
  }
}
