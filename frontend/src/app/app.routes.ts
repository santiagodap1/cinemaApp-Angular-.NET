import { Routes } from '@angular/router';
import { adminKeyGuard } from './core/auth/admin-key.guard';
import { AdminDashboardComponent } from './features/admin/pages/dashboard/admin-dashboard.component';
import { AdminLoginComponent } from './features/admin/pages/login/admin-login.component';
import { AdminCinemaSitesComponent } from './features/admin/pages/cinema-sites/admin-cinema-sites.component';
import { AdminAuditoriumsComponent } from './features/admin/pages/auditoriums/admin-auditoriums.component';
import { AdminSeatsComponent } from './features/admin/pages/seats/admin-seats.component';
import { AdminMoviesComponent } from './features/admin/pages/movies/admin-movies.component';
import { AdminScreeningsComponent } from './features/admin/pages/screenings/admin-screenings.component';
import { AdminSeedComponent } from './features/admin/pages/seed/admin-seed.component';
import { AuditoriumsPageComponent } from './features/auditoriums/auditoriums-page.component';
import { CinemaSitesPageComponent } from './features/cinema-sites/cinema-sites-page.component';
import { MoviesPageComponent } from './features/movies/movies-page.component';
import { OccupancyPageComponent } from './features/occupancy/occupancy-page.component';
import { ReservationsPageComponent } from './features/reservations/reservations-page.component';
import { ScreeningsPageComponent } from './features/screenings/screenings-page.component';
import { SeatMapPageComponent } from './features/seat-map/seat-map-page.component';
import { SeatsPageComponent } from './features/seats/seats-page.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { PublicLayoutComponent } from './layouts/public-layout/public-layout.component';

export const routes: Routes = [
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'cinema-sites' },
      { path: 'cinema-sites', component: CinemaSitesPageComponent },
      { path: 'auditoriums', component: AuditoriumsPageComponent },
      { path: 'seats', component: SeatsPageComponent },
      { path: 'movies', component: MoviesPageComponent },
      { path: 'screenings', component: ScreeningsPageComponent },
      { path: 'seat-map', component: SeatMapPageComponent },
      { path: 'reservations', component: ReservationsPageComponent },
      { path: 'occupancy', component: OccupancyPageComponent }
    ]
  },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
      { path: 'login', component: AdminLoginComponent },
      { path: 'cinema-sites', component: AdminCinemaSitesComponent, canActivate: [adminKeyGuard] },
      { path: 'auditoriums', component: AdminAuditoriumsComponent, canActivate: [adminKeyGuard] },
      { path: 'seats', component: AdminSeatsComponent, canActivate: [adminKeyGuard] },
      { path: 'movies', component: AdminMoviesComponent, canActivate: [adminKeyGuard] },
      { path: 'screenings', component: AdminScreeningsComponent, canActivate: [adminKeyGuard] },
      { path: 'seed', component: AdminSeedComponent, canActivate: [adminKeyGuard] },
      { path: '', component: AdminDashboardComponent, canActivate: [adminKeyGuard] }
    ]
  }
];
