import { Injectable } from '@angular/core';
import { ApiClient } from '../../core/api/api-client.service';
import { ScreeningOccupancy } from './occupancy.models';

@Injectable({ providedIn: 'root' })
export class OccupancyService {
  constructor(private readonly apiClient: ApiClient) {}

  getByScreening(screeningId: string) {
    return this.apiClient.get<ScreeningOccupancy>(`/api/screenings/${screeningId}/occupancy`);
  }
}
