export interface Movie {
  id: string;
  title: string;
  durationMinutes: number;
  rating: string;
  synopsis: string;
  director: string;
  cast: string;
  language: string;
  country: string;
  releaseDate?: string | null;
}

export interface CreateMovieRequest {
  title: string;
  durationMinutes: number;
  rating: string;
  synopsis?: string | null;
  director?: string | null;
  cast?: string | null;
  language?: string | null;
  country?: string | null;
  releaseDate?: string | null;
}
