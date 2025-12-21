export type SeatAvailabilityStatus = 0 | 1 | 2;

export interface SeatMapItem {
  seatId: string;
  row: string;
  number: number;
  status: SeatAvailabilityStatus;
}

export interface SeatMapResponse {
  auditoriumId: string;
  capacity: number;
  screeningId: string;
  startsAt: string;
  seats: SeatMapItem[];
}

export const seatStatusLabels: Record<SeatAvailabilityStatus, string> = {
  0: 'Available',
  1: 'Held',
  2: 'Reserved'
};
