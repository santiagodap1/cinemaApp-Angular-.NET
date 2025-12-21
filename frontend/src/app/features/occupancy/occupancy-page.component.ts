import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OccupancyService } from './occupancy.service';
import { ScreeningOccupancy } from './occupancy.models';

@Component({
  selector: 'app-occupancy-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './occupancy-page.component.html',
  styleUrl: './occupancy-page.component.scss'
})
export class OccupancyPageComponent {
  screeningId = '';
  occupancy: ScreeningOccupancy | null = null;
  isLoading = false;
  error = '';

  constructor(private readonly occupancyService: OccupancyService) {}

  load() {
    if (!this.screeningId.trim()) {
      this.error = 'Screening ID is required.';
      return;
    }

    this.isLoading = true;
    this.error = '';
    this.occupancy = null;

    this.occupancyService.getByScreening(this.screeningId.trim()).subscribe({
      next: (data) => {
        this.occupancy = data;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load occupancy.';
        this.isLoading = false;
      }
    });
  }
}
