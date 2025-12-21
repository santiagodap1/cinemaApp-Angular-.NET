import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReservationsService } from './reservations.service';
import { ReservationResponse } from './reservations.models';

@Component({
  selector: 'app-reservations-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './reservations-page.component.html',
  styleUrl: './reservations-page.component.scss'
})
export class ReservationsPageComponent {
  isLoading = false;
  error = '';
  result: ReservationResponse | null = null;
  reservationId = '';

  form = this.fb.group({
    screeningId: ['', [Validators.required]],
    customerEmail: ['', [Validators.required, Validators.email]],
    customerFullName: ['', [Validators.required]],
    seatIds: ['', [Validators.required]]
  });

  constructor(private readonly fb: FormBuilder, private readonly reservationsService: ReservationsService) {}

  create() {
    if (this.form.invalid) {
      return;
    }

    const seatIds = (this.form.value.seatIds ?? '')
      .split(',')
      .map((value) => value.trim())
      .filter((value) => value.length > 0);

    this.isLoading = true;
    this.error = '';
    this.result = null;

    this.reservationsService.create({
      screeningId: this.form.value.screeningId ?? '',
      customerEmail: this.form.value.customerEmail ?? '',
      customerFullName: this.form.value.customerFullName ?? '',
      seatIds
    }).subscribe({
      next: (data) => {
        this.result = data;
        this.reservationId = data.id;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to create reservation.';
        this.isLoading = false;
      }
    });
  }

  confirm() {
    if (!this.reservationId.trim()) {
      this.error = 'Reservation ID is required.';
      return;
    }

    this.reservationsService.confirm(this.reservationId.trim()).subscribe({
      next: (data) => {
        this.result = data;
      },
      error: () => {
        this.error = 'Failed to confirm reservation.';
      }
    });
  }

  cancel() {
    if (!this.reservationId.trim()) {
      this.error = 'Reservation ID is required.';
      return;
    }

    this.reservationsService.cancel(this.reservationId.trim()).subscribe({
      next: (data) => {
        this.result = data;
      },
      error: () => {
        this.error = 'Failed to cancel reservation.';
      }
    });
  }
}
