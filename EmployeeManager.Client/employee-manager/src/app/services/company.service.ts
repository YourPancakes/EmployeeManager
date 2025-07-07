import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Company, CompanyStatistics } from '../models/company.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = 'http://localhost:5000/api/v1/company';

  constructor(private http: HttpClient) { }

  getCompany(): Observable<Company> {
    return this.http.get<Company>(this.apiUrl);
  }

  getCompanyStatistics(): Observable<CompanyStatistics> {
    return this.http.get<CompanyStatistics>(`${this.apiUrl}/statistics`);
  }
} 