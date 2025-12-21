import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SeatMapService } from './seat-map.service';
import { SeatMapResponse, seatStatusLabels } from './seat-map.models';

@Component({
  selector: 'app-seat-map-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './seat-map-page.component.html',
  styleUrl: './seat-map-page.component.scss'
})
export class SeatMapPageComponent {
  screeningId = '';
  seatMap: SeatMapResponse | null = null;
  isLoading = false;
  error = '';
  seatStatusLabels = seatStatusLabels;

  constructor(private readonly seatMapService: SeatMapService) {}

  load() {
    if (!this.screeningId.trim()) {
      this.error = 'Screening ID is required.';
      return;
    }

    this.isLoading = true;
    this.error = '';
    this.seatMap = null;

    this.seatMapService.getByScreening(this.screeningId.trim()).subscribe({
      next: (data) => {
        this.seatMap = data;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load seat map.';
        this.isLoading = false;
      }
    });
  }
}
