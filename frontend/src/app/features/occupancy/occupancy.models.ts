export interface ScreeningOccupancy {
  screeningId: string;
  capacity: number;
  reservedCount: number;
  heldCount: number;
  availableCount: number;
}
