import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ScreeningsService } from '../../../screenings/screenings.service';
import { formatLabels, MovieFormat, Screening } from '../../../screenings/screenings.models';

@Component({
  selector: 'app-admin-screenings',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './admin-screenings.component.html',
  styleUrl: './admin-screenings.component.scss'
})
export class AdminScreeningsComponent {
  screenings: Screening[] = [];
  isLoading = false;
  error = '';
  page = 1;
  pageSize = 8;
  movieIdFilter = '';
  cinemaSiteIdFilter = '';
  dateFilter = '';
  formatLabels = formatLabels;

  formatOptions: Array<{ value: MovieFormat; label: string }> = [
    { value: 0, label: '2D' },
    { value: 1, label: '3D' },
    { value: 2, label: 'IMAX' }
  ];

  form = this.fb.group({
    movieId: ['', [Validators.required]],
    auditoriumId: ['', [Validators.required]],
    startsAt: ['', [Validators.required]],
    endsAt: ['', [Validators.required]],
    format: [0 as MovieFormat, [Validators.required]]
  });

  constructor(private readonly fb: FormBuilder, private readonly screeningsService: ScreeningsService) {}

  load() {
    this.isLoading = true;
    this.error = '';

    this.screeningsService.getAll({
      movieId: this.movieIdFilter,
      cinemaSiteId: this.cinemaSiteIdFilter,
      date: this.dateFilter
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
    this.movieIdFilter = '';
    this.cinemaSiteIdFilter = '';
    this.dateFilter = '';
    this.applyFilter();
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const payload = {
      movieId: this.form.value.movieId ?? '',
      auditoriumId: this.form.value.auditoriumId ?? '',
      startsAt: this.form.value.startsAt ?? '',
      endsAt: this.form.value.endsAt ?? '',
      format: this.form.value.format ?? 0
    };

    this.screeningsService.create(payload).subscribe({
      next: () => {
        this.form.reset({ format: 0 });
        this.load();
      },
      error: () => {
        this.error = 'Failed to create screening.';
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
