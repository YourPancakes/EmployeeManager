import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Employee, EmployeeForm } from '../../../models/employee.model';
import { Department } from '../../../models/department.model';
import { EmployeeService } from '../../../services/employee.service';
import { DepartmentService } from '../../../services/department.service';

@Component({
  selector: 'app-employee-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './employee-dialog.component.html',
  styleUrls: ['./employee-dialog.component.css']
})
export class EmployeeModalComponent implements OnInit {
  @Input() employee: EmployeeForm = {} as EmployeeForm;
  @Input() isEditMode: boolean = false;
  @Output() saved = new EventEmitter<Employee>();
  @Output() closed = new EventEmitter<void>();

  departments: Department[] = [];
  error: string | null = null;
  isLoading: boolean = false;

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.departmentService.getAllDepartments().subscribe({
      next: (departments: Department[]) => {
        this.departments = departments;
      },
      error: (error: any) => {
        console.error('Error loading departments:', error);
        this.error = 'Failed to load departments';
      }
    });
  }

  save(form: NgForm): void {
    this.error = null;
    if (form.invalid) {
      form.control.markAllAsTouched();
      const missingFields = [];
      if (!this.employee.firstName) missingFields.push('First Name');
      if (!this.employee.lastName) missingFields.push('Last Name');
      if (!this.employee.departmentId) missingFields.push('Department');
      if (!this.employee.birthDate) missingFields.push('Birth Date');
      if (!this.employee.hireDate) missingFields.push('Hire Date');
      if (!this.employee.salary && this.employee.salary !== 0) missingFields.push('Salary');
      if (missingFields.length > 0) {
        this.error = 'The following fields are required: ' + missingFields.join(', ');
      }
      return;
    }
    this.isLoading = true;
    // Convert form data to API format
    const employeeData = {
      departmentId: this.employee.departmentId,
      fullName: `${this.employee.firstName} ${this.employee.lastName}${this.employee.middleName ? ' ' + this.employee.middleName : ''}`.trim(),
      birthDate: new Date(this.employee.birthDate),
      hireDate: new Date(this.employee.hireDate),
      salary: this.employee.salary
    };
    const operation = this.isEditMode 
      ? this.employeeService.updateEmployee(this.employee.employeeId!, employeeData)
      : this.employeeService.createEmployee(employeeData);
    operation.subscribe({
      next: (savedEmployee) => {
        this.isLoading = false;
        this.saved.emit(savedEmployee);
        this.close();
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Error saving employee:', error);
        if (error.error && typeof error.error === 'object') {
          const validationErrors = error.error.errors || error.error;
          const errorMessages = Object.values(validationErrors).flat();
          this.error = Array.isArray(errorMessages) ? errorMessages.join(', ') : errorMessages;
        } else if (error.error && typeof error.error === 'string') {
          this.error = error.error;
        } else {
          this.error = this.isEditMode ? 'Failed to update employee' : 'Failed to create employee';
        }
      }
    });
  }

  close(): void {
    this.closed.emit();
  }
} 