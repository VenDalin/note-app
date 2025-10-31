export interface Note {
  id: number;
  userId: number;
  title: string;
  content?: string;
  createdAt: string;
  updatedAt?: string;
}
export interface PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
}
export interface NoteQueryParams {
  q?: string;
  sort?: string;
  page?: number;
  pageSize?: number;
}
