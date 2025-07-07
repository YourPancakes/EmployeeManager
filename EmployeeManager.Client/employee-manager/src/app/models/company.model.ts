export interface Company {
  companyId: number;
  name: string;
  founded: number;
  industry: string;
  description: string;
  headquarters: string;
  website: string;
}

export interface CompanyStatistics {
  totalEmployees: number;
  departments: number;
  foundedYears: number;
  projectsCompleted: number;
  clientSatisfaction: number;
  annualRevenue: string;
} 