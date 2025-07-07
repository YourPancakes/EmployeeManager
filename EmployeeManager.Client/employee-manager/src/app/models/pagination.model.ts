export interface PaginationMetadata {
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

export interface PaginatedResult<T> {
  data: T[];
  pagination: PaginationMetadata;
}

export interface FlatPaginatedResult<T> {
  data: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNext: boolean;
  hasPrevious: boolean;
}

export interface PaginationParameters {
  page: number;
  pageSize: number;
}

export interface SearchParameters {
  department?: string;
  fullName?: string;
  birthDate?: string;
  hireDate?: string;
  salary?: string;
} 