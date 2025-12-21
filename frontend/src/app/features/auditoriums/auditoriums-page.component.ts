import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuditoriumsService } from './auditoriums.service';
import { Auditorium } from './auditoriums.models';

@Component({
  selector: 'app-auditoriums-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './auditoriums-page.component.html',
  styleUrl: './auditoriums-page.component.scss'
})
export class AuditoriumsPageComponent implements OnInit {
  auditoriums: Auditorium[] = [];
  isLoading = false;
  error = '';
  page = 1;
  pageSize = 10;
  cinemaSiteId = '';

  constructor(private readonly auditoriumsService: AuditoriumsService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.isLoading = true;
    this.error = '';

    this.auditoriumsService.getAll(this.cinemaSiteId || null, this.page, this.pageSize).subscribe({
      next: (data) => {
        this.auditoriums = data;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load auditoriums.';
        this.isLoading = false;
      }
    });
  }

  applyFilter() {
    this.page = 1;
    this.load();
  }

  clearFilter() {
    this.cinemaSiteId = '';
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
