import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ScreeningsService } from './screenings.service';
import { formatLabels, Screening, ScreeningOccupancy } from './screenings.models';
import { EmptyStateComponent } from '../../shared/ui/empty-state/empty-state.component';

@Component({
  selector: 'app-screenings-page',
  standalone: true,
  imports: [CommonModule, FormsModule, EmptyStateComponent],
  templateUrl: './screenings-page.component.html',
  styleUrl: './screenings-page.component.scss'
})
export class ScreeningsPageComponent implements OnInit {
  screenings: Screening[] = [];
  occupancy: Record<string, ScreeningOccupancy> = {};
  isLoading = false;
  error = '';
  page = 1;
  pageSize = 8;
  movieId = '';
  cinemaSiteId = '';
  date = '';
  formatLabels = formatLabels;

  constructor(private readonly screeningsService: ScreeningsService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.isLoading = true;
    this.error = '';

    this.screeningsService.getAll({
      movieId: this.movieId,
      cinemaSiteId: this.cinemaSiteId,
      date: this.date
    }, this.page, this.pageSize).subscribe({
      next: (data) => {
        this.screenings = data;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load screenings.';
        this.isLoading = false;
      }
    });
  }

  applyFilter() {
    this.page = 1;
    this.load();
  }

  clearFilter() {
    this.movieId = '';
    this.cinemaSiteId = '';
    this.date = '';
    this.applyFilter();
  }

  showOccupancy(screeningId: string) {
    this.screeningsService.getOccupancy(screeningId).subscribe({
      next: (data) => {
        this.occupancy[screeningId] = data;
      },
      error: () => {
        this.error = 'Failed to load occupancy.';
      }
    });
  }

  nextPage() {
    this.page += 1;
    this.load();
  }

  previousPage() {
    if (this.page === 1) {
      return;
    }

    this.page -= 1;
    this.load();
  }
}
