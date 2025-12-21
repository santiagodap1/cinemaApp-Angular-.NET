import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SeatsService } from './seats.service';
import { Seat } from './seats.models';

@Component({
  selector: 'app-seats-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './seats-page.component.html',
  styleUrl: './seats-page.component.scss'
})
export class SeatsPageComponent {
  auditoriumId = '';
  seats: Seat[] = [];
  isLoading = false;
  error = '';
  page = 1;
  pageSize = 12;

  constructor(private readonly seatsService: SeatsService) {}

  load() {
    if (!this.auditoriumId.trim()) {
      this.error = 'Auditorium ID is required.';
      return;
    }

    this.isLoading = true;
    this.error = '';

    this.seatsService.getByAuditorium(this.auditoriumId.trim(), this.page, this.pageSize).subscribe({
      next: (data) => {
        this.seats = data;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load seats.';
        this.isLoading = false;
      }
    });
  }

  apply() {
    this.page = 1;
    this.load();
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
