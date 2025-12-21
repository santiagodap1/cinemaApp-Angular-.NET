import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CinemaSitesService } from './cinema-sites.service';
import { CinemaSite } from './cinema-sites.models';
import { EmptyStateComponent } from '../../shared/ui/empty-state/empty-state.component';

@Component({
  selector: 'app-cinema-sites-page',
  standalone: true,
  imports: [CommonModule, EmptyStateComponent],
  templateUrl: './cinema-sites-page.component.html',
  styleUrl: './cinema-sites-page.component.scss'
})
export class CinemaSitesPageComponent implements OnInit {
  sites: CinemaSite[] = [];
  isLoading = false;
  error = '';
  page = 1;
  pageSize = 10;

  constructor(private readonly cinemaSitesService: CinemaSitesService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.isLoading = true;
    this.error = '';

    this.cinemaSitesService.getAll(this.page, this.pageSize).subscribe({
      next: (data) => {
        this.sites = data;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load cinema sites.';
        this.isLoading = false;
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
