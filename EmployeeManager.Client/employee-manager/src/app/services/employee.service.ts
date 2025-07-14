import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Employee, EmployeeCreate, EmployeeUpdate } from '../models/employee.model';
import { PaginatedResult, PaginationParameters, SearchParameters, FlatPaginatedResult } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'http://localhost:5000/api/v1/employees';

  constructor(private http: HttpClient) { }

  getAllEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl).pipe(
      map(employees => employees.map(employee => ({
        ...employee,
        birthDate: new Date(employee.birthDate),
        hireDate: new Date(employee.hireDate)
      })))
    );
  }

  getEmployeesPaginated(parameters: PaginationParameters, searchParameters?: SearchParameters, sortField?: string, sortDirection?: string):
    Observable<PaginatedResult<Employee>> {
    let params = new HttpParams()
      .set('page', parameters.page.toString())
      .set('pageSize', parameters.pageSize.toString());

    // Add search parameters if provided
    if (searchParameters) {
      if (searchParameters.department) {
        params = params.set('department', searchParameters.department);
      }
      if (searchParameters.fullName) {
        params = params.set('fullName', searchParameters.fullName);
      }
      if (searchParameters.birthDate) {
        params = params.set('birthDate', searchParameters.birthDate);
      }
      if (searchParameters.hireDate) {
        params = params.set('hireDate', searchParameters.hireDate);
      }
      if (searchParameters.salary) {
        params = params.set('salary', searchParameters.salary);
      }
    }

    /*if (searchParameters) {
        Object.entries(searchParameters).forEach(([key, value]) => {
          if (value !== undefined && value !== null) {
            params = params.set(key, value);
          }
        });
      } */

    // Add sorting parameters if provided
    if (sortField) {
      params = params.set('sortField', sortField);
    }
    if (sortDirection) {
      params = params.set('sortDirection', sortDirection);
    }

    return this.http.get<FlatPaginatedResult<Employee>>(`${this.apiUrl}/paginated`, { params }).pipe(
      map(result => {
        return {
          data: result.data.map(employee => ({
            ...employee,
            birthDate: new Date(employee.birthDate),
            hireDate: new Date(employee.hireDate)
          })),
          pagination: {
            page: result.page,
            pageSize: result.pageSize,
            totalItems: result.totalItems,
            totalPages: result.totalPages,
            hasNext: result.hasNext,
            hasPrevious: result.hasPrevious
          }
        };
      })
    );
  }

  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`).pipe(
      map(employee => ({
        ...employee,
        birthDate: new Date(employee.birthDate),
        hireDate: new Date(employee.hireDate)
      }))
    );
  }

  createEmployee(employee: EmployeeCreate): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, employee).pipe(
      map(employee => ({
        ...employee,
        birthDate: new Date(employee.birthDate),
        hireDate: new Date(employee.hireDate)
      }))
    );
  }

  updateEmployee(id: number, employee: EmployeeUpdate): Observable<Employee> {
    return this.http.put<Employee>(`${this.apiUrl}/${id}`, employee).pipe(
      map(employee => ({
        ...employee,
        birthDate: new Date(employee.birthDate),
        hireDate: new Date(employee.hireDate)
      }))
    );
  }

  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
