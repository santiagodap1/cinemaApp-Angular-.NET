import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { SeatsService } from '../../../seats/seats.service';
import { Seat } from '../../../seats/seats.models';

@Component({
  selector: 'app-admin-seats',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './admin-seats.component.html',
  styleUrl: './admin-seats.component.scss'
})
export class AdminSeatsComponent {
  auditoriumId = '';
  seats: Seat[] = [];
  isLoading = false;
  error = '';
  page = 1;
  pageSize = 12;

  form = this.fb.group({
    auditoriumId: ['', [Validators.required]],
    row: ['', [Validators.required, Validators.maxLength(5)]],
    number: [1, [Validators.required, Validators.min(1), Validators.max(500)]]
  });

  constructor(private readonly fb: FormBuilder, private readonly seatsService: SeatsService) {}

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

  save() {
    if (this.form.invalid) {
      return;
    }

    const payload = {
      auditoriumId: this.form.value.auditoriumId ?? '',
      row: this.form.value.row ?? '',
      number: Number(this.form.value.number ?? 0)
    };

    this.seatsService.create(payload).subscribe({
      next: () => {
        this.form.reset({ number: 1 });
        this.auditoriumId = payload.auditoriumId;
        this.load();
      },
      error: () => {
        this.error = 'Failed to create seat.';
      }
    });
  }

  remove(seat: Seat) {
    if (!confirm(`Delete seat ${seat.row}${seat.number}?`)) {
      return;
    }

    this.seatsService.delete(seat.auditoriumId, seat.id).subscribe({
      next: () => this.load(),
      error: () => {
        this.error = 'Failed to delete seat.';
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
