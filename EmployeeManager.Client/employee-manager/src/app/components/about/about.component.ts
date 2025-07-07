import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CompanyService } from '../../services/company.service';
import { Company, CompanyStatistics } from '../../models/company.model';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  company: Company | null = null;
  statistics: CompanyStatistics | null = null;
  error: string | null = null;

  constructor(private companyService: CompanyService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.error = null;
    
    this.companyService.getCompany().subscribe({
      next: (company) => {
        this.company = company;
      },
      error: (error) => {
        console.error('Error loading company:', error);
        this.error = 'Failed to load company information';
      }
    });

    this.companyService.getCompanyStatistics().subscribe({
      next: (statistics) => {
        this.statistics = statistics;
      },
      error: (error) => {
        console.error('Error loading statistics:', error);
        this.error = 'Failed to load company statistics';
      }
    });
  }
} 