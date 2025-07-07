export interface Employee {
  employeeId: number;
  departmentId: number;
  departmentName: string;
  fullName: string;
  birthDate: Date;
  hireDate: Date;
  salary: number;
}

export interface EmployeeForm {
  employeeId?: number;
  departmentId: number;
  firstName: string;
  lastName: string;
  middleName?: string;
  birthDate: Date | string;
  hireDate: Date | string;
  salary: number;
}

export interface EmployeeCreate {
  departmentId: number;
  fullName: string;
  birthDate: Date;
  hireDate: Date;
  salary: number;
}

export interface EmployeeUpdate {
  departmentId: number;
  fullName: string;
  birthDate: Date;
  hireDate: Date;
  salary: number;
} 