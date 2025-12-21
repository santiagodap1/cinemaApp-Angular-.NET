import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CinemaSitesService } from '../../../cinema-sites/cinema-sites.service';
import { CinemaSite } from '../../../cinema-sites/cinema-sites.models';

@Component({
  selector: 'app-admin-cinema-sites',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-cinema-sites.component.html',
  styleUrl: './admin-cinema-sites.component.scss'
})
export class AdminCinemaSitesComponent implements OnInit {
  sites: CinemaSite[] = [];
  isLoading = false;
  error = '';
  page = 1;
  pageSize = 10;

  form = this.fb.group({
    id: [''],
    name: ['', [Validators.required, Validators.maxLength(200)]],
    street: ['', [Validators.required, Validators.maxLength(200)]],
    city: ['', [Validators.required, Validators.maxLength(100)]],
    state: ['', [Validators.required, Validators.maxLength(100)]],
    country: ['', [Validators.required, Validators.maxLength(100)]],
    postalCode: ['', [Validators.required, Validators.maxLength(20)]]
  });

  constructor(private readonly fb: FormBuilder, private readonly service: CinemaSitesService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.isLoading = true;
    this.error = '';

    this.service.getAll(this.page, this.pageSize).subscribe({
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

  edit(site: CinemaSite) {
    this.form.patchValue({
      id: site.id,
      name: site.name,
      street: site.address.street,
      city: site.address.city,
      state: site.address.state,
      country: site.address.country,
      postalCode: site.address.postalCode
    });
  }

  reset() {
    this.form.reset();
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const payload = {
      name: this.form.value.name ?? '',
      address: {
        street: this.form.value.street ?? '',
        city: this.form.value.city ?? '',
        state: this.form.value.state ?? '',
        country: this.form.value.country ?? '',
        postalCode: this.form.value.postalCode ?? ''
      }
    };

    const id = this.form.value.id;
    const request$ = id
      ? this.service.update(id, payload)
      : this.service.create(payload);

    request$.subscribe({
      next: () => {
        this.reset();
        this.load();
      },
      error: () => {
        this.error = 'Failed to save cinema site.';
      }
    });
  }

  remove(site: CinemaSite) {
    if (!confirm(`Delete ${site.name}?`)) {
      return;
    }

    this.service.delete(site.id).subscribe({
      next: () => this.load(),
      error: () => {
        this.error = 'Failed to delete cinema site.';
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
