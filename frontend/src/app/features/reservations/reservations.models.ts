export interface CreateReservationRequest {
  screeningId: string;
  customerEmail: string;
  customerFullName: string;
  seatIds: string[];
}

export interface ReservationResponse {
  id: string;
  screeningId: string;
  customerId: string;
  status: number;
  totalAmount: number;
  reservedAt: string;
  seatIds: string[];
}
