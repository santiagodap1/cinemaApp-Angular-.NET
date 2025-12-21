export interface Auditorium {
  id: string;
  cinemaSiteId: string;
  name: string;
  capacity: number;
}

export interface CreateAuditoriumRequest {
  cinemaSiteId: string;
  name: string;
  capacity: number;
}

export interface UpdateAuditoriumRequest {
  name: string;
  capacity: number;
}
