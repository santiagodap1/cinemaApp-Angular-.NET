import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuditoriumsService } from '../../../auditoriums/auditoriums.service';
import { Auditorium } from '../../../auditoriums/auditoriums.models';

@Component({
  selector: 'app-admin-auditoriums',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './admin-auditoriums.component.html',
  styleUrl: './admin-auditoriums.component.scss'
})
export class AdminAuditoriumsComponent implements OnInit {
  auditoriums: Auditorium[] = [];
  isLoading = false;
  error = '';
  page = 1;
  pageSize = 10;
  filterCinemaSiteId = '';

  form = this.fb.group({
    id: [''],
    cinemaSiteId: ['', [Validators.required]],
    name: ['', [Validators.required, Validators.maxLength(100)]],
    capacity: [100, [Validators.required, Validators.min(1), Validators.max(1000)]]
  });

  constructor(private readonly fb: FormBuilder, private readonly service: AuditoriumsService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.isLoading = true;
    this.error = '';

    this.service.getAll(this.filterCinemaSiteId || null, this.page, this.pageSize).subscribe({
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
    this.filterCinemaSiteId = '';
    this.applyFilter();
  }

  edit(auditorium: Auditorium) {
    this.form.patchValue({
      id: auditorium.id,
      cinemaSiteId: auditorium.cinemaSiteId,
      name: auditorium.name,
      capacity: auditorium.capacity
    });
  }

  reset() {
    this.form.reset({ capacity: 100 });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const payload = {
      cinemaSiteId: this.form.value.cinemaSiteId ?? '',
      name: this.form.value.name ?? '',
      capacity: Number(this.form.value.capacity ?? 0)
    };

    const id = this.form.value.id;
    const request$ = id
      ? this.service.update(id, { name: payload.name, capacity: payload.capacity })
      : this.service.create(payload);

    request$.subscribe({
      next: () => {
        this.reset();
        this.load();
      },
      error: () => {
        this.error = 'Failed to save auditorium.';
      }
    });
  }

  remove(auditorium: Auditorium) {
    if (!confirm(`Delete ${auditorium.name}?`)) {
      return;
    }

    this.service.delete(auditorium.id).subscribe({
      next: () => this.load(),
      error: () => {
        this.error = 'Failed to delete auditorium.';
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
