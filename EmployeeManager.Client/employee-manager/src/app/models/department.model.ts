export interface Department {
  departmentId: number;
  companyId: number;
  companyName: string;
  name: string;
}

export interface DepartmentCreate {
  companyId: number;
  name: string;
}

export interface DepartmentUpdate {
  name: string;
} 