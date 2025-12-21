import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MoviesService } from '../../../movies/movies.service';
import { Movie } from '../../../movies/movies.models';

@Component({
  selector: 'app-admin-movies',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './admin-movies.component.html',
  styleUrl: './admin-movies.component.scss'
})
export class AdminMoviesComponent {
  movies: Movie[] = [];
  isLoading = false;
  error = '';
  page = 1;
  pageSize = 8;
  titleFilter = '';

  form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    durationMinutes: [90, [Validators.required, Validators.min(1), Validators.max(400)]],
    rating: ['', [Validators.required, Validators.maxLength(20)]],
    synopsis: ['', [Validators.maxLength(2000)]],
    director: ['', [Validators.maxLength(200)]],
    cast: ['', [Validators.maxLength(500)]],
    language: ['', [Validators.maxLength(100)]],
    country: ['', [Validators.maxLength(100)]],
    releaseDate: ['']
  });

  constructor(private readonly fb: FormBuilder, private readonly moviesService: MoviesService) {}

  load() {
    this.isLoading = true;
    this.error = '';

    this.moviesService.getAll(this.titleFilter, this.page, this.pageSize).subscribe({
      next: (data) => {
        this.movies = data;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load movies.';
        this.isLoading = false;
      }
    });
  }

  applyFilter() {
    this.page = 1;
    this.load();
  }

  clearFilter() {
    this.titleFilter = '';
    this.applyFilter();
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const payload = {
      title: this.form.value.title ?? '',
      durationMinutes: Number(this.form.value.durationMinutes ?? 0),
      rating: this.form.value.rating ?? '',
      synopsis: this.form.value.synopsis || null,
      director: this.form.value.director || null,
      cast: this.form.value.cast || null,
      language: this.form.value.language || null,
      country: this.form.value.country || null,
      releaseDate: this.form.value.releaseDate || null
    };

    this.moviesService.create(payload).subscribe({
      next: () => {
        this.form.reset({ durationMinutes: 90 });
        this.load();
      },
      error: () => {
        this.error = 'Failed to create movie.';
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
