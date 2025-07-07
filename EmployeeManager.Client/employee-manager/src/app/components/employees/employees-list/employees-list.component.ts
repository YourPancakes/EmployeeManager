import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModal, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { EmployeeService } from '../../../services/employee.service';
import { DepartmentService } from '../../../services/department.service';
import { Employee } from '../../../models/employee.model';
import { Department } from '../../../models/department.model';
import { PaginatedResult, PaginationMetadata, SearchParameters } from '../../../models/pagination.model';
import { EmployeeModalComponent } from '../employee-dialog/employee-dialog.component';
import { DeleteModalComponent } from '../delete-confirm-dialog/delete-confirm-dialog.component';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [CommonModule, FormsModule, NgbPaginationModule],
  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.css']
})
export class EmployeesComponent implements OnInit, OnDestroy {
  employees: Employee[] = [];
  departments: Department[] = [];
  error: string | null = null;
  pagination: PaginationMetadata | null = null;
  currentPage = 1;
  pageSize = 10;
  isLoading = false;
  filters = {
    department: '',
    fullName: '',
    birthDate: '',
    hireDate: '',
    salary: ''
  };
  sortField = 'fullName';
  sortDirection = 'asc';
  Math = Math;
  private searchSubject = new Subject<void>();
  private destroy$ = new Subject<void>();

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.loadData();
    
    // Setup debounced search
    this.searchSubject.pipe(
      debounceTime(300),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.applyFilters();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadData(): void {
    this.loadEmployees();
    this.loadDepartments();
  }

  loadEmployees(): void {
    this.isLoading = true;
    this.error = null;
    
    const hasActiveFilters = Object.values(this.filters).some(value => value && value.toString().trim() !== '');
    
    // Always use server-side pagination with search
    const searchParams: SearchParameters | undefined = hasActiveFilters ? {
      department: this.filters.department || undefined,
      fullName: this.filters.fullName || undefined,
      birthDate: this.filters.birthDate || undefined,
      hireDate: this.filters.hireDate || undefined,
      salary: this.filters.salary || undefined
    } : undefined;

    this.employeeService.getEmployeesPaginated({
      page: this.currentPage,
      pageSize: this.pageSize
    }, searchParams, this.sortField, this.sortDirection).subscribe({
      next: (result: PaginatedResult<Employee>) => {
        this.employees = result.data;
        this.pagination = result.pagination;
        this.isLoading = false;
      },
      error: (error: any) => {
        this.error = 'Failed to load employees list';
        this.isLoading = false;
      }
    });
  }

  loadDepartments(): void {
    this.departmentService.getAllDepartments().subscribe({
      next: (departments: Department[]) => {
        this.departments = departments;
      },
      error: (error: any) => {
        this.error = 'Failed to load departments list';
      }
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadEmployees();
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.loadEmployees();
  }

  applyFilters(): void {
    // Reset to first page when applying filters
    this.currentPage = 1;
    // Reload data with new filters
    this.loadEmployees();
  }

  onFilterChange(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  sort(field: string): void {
    if (this.sortField === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortDirection = 'asc';
    }
    this.loadEmployees();
  }

  openCreateModal(): void {
    const modalRef = this.modalService.open(EmployeeModalComponent, { size: 'lg' });
    modalRef.componentInstance.isEditMode = false;
    modalRef.componentInstance.saved.subscribe(() => {
      this.loadEmployees();
    });
    modalRef.componentInstance.closed.subscribe(() => {
      modalRef.close();
    });
  }

  openEditModal(employee: Employee): void {
    const modalRef = this.modalService.open(EmployeeModalComponent, { size: 'lg' });
    const nameParts = employee.fullName.trim().split(/\s+/);
    const formatDateForInput = (date: Date): string => {
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, '0');
      const day = String(date.getDate()).padStart(2, '0');
      return `${year}-${month}-${day}`;
    };
    modalRef.componentInstance.employee = {
      employeeId: employee.employeeId,
      departmentId: employee.departmentId,
      firstName: nameParts[0] || '',
      lastName: nameParts[1] || '',
      middleName: nameParts.length > 2 ? nameParts.slice(2).join(' ') : undefined,
      birthDate: formatDateForInput(employee.birthDate),
      hireDate: formatDateForInput(employee.hireDate),
      salary: employee.salary
    };
    modalRef.componentInstance.isEditMode = true;
    modalRef.componentInstance.saved.subscribe(() => {
      this.loadEmployees();
    });
    modalRef.componentInstance.closed.subscribe(() => {
      modalRef.close();
    });
  }

  openDeleteModal(employee: Employee): void {
    const modalRef = this.modalService.open(DeleteModalComponent);
    modalRef.componentInstance.employee = employee;
    modalRef.componentInstance.deleted.subscribe(() => {
      this.loadEmployees();
    });
    modalRef.componentInstance.closed.subscribe(() => {
      modalRef.close();
    });
  }
} 