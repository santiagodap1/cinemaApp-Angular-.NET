import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MoviesService } from './movies.service';
import { Movie } from './movies.models';
import { EmptyStateComponent } from '../../shared/ui/empty-state/empty-state.component';

@Component({
  selector: 'app-movies-page',
  standalone: true,
  imports: [CommonModule, FormsModule, EmptyStateComponent],
  templateUrl: './movies-page.component.html',
  styleUrl: './movies-page.component.scss'
})
export class MoviesPageComponent implements OnInit {
  movies: Movie[] = [];
  isLoading = false;
  error = '';
  page = 1;
  pageSize = 8;
  titleFilter = '';

  constructor(private readonly moviesService: MoviesService) {}

  ngOnInit() {
    this.load();
  }

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
