import { Injectable } from '@angular/core';
import { ApiClient } from '../../core/api/api-client.service';
import { CreateMovieRequest, Movie } from './movies.models';

@Injectable({ providedIn: 'root' })
export class MoviesService {
  constructor(private readonly apiClient: ApiClient) {}

  getAll(title: string, page: number, pageSize: number) {
    return this.apiClient.get<Movie[]>('/api/movies', {
      title: title || undefined,
      page,
      pageSize
    });
  }

  create(payload: CreateMovieRequest) {
    return this.apiClient.post<Movie>('/api/movies', payload);
  }
}
