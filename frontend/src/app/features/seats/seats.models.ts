export interface Seat {
  id: string;
  auditoriumId: string;
  row: string;
  number: number;
}

export interface CreateSeatRequest {
  auditoriumId: string;
  row: string;
  number: number;
}
