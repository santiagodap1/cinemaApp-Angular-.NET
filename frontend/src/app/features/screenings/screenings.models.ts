export type MovieFormat = 0 | 1 | 2;

export interface Screening {
  id: string;
  movieId: string;
  auditoriumId: string;
  startsAt: string;
  endsAt: string;
  format: MovieFormat;
}

export interface ScreeningOccupancy {
  screeningId: string;
  capacity: number;
  reservedCount: number;
  heldCount: number;
  availableCount: number;
}

export interface CreateScreeningRequest {
  movieId: string;
  auditoriumId: string;
  startsAt: string;
  endsAt: string;
  format: MovieFormat;
}

export const formatLabels: Record<MovieFormat, string> = {
  0: '2D',
  1: '3D',
  2: 'IMAX'
};
