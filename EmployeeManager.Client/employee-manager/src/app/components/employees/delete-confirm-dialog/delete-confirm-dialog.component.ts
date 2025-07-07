import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Employee } from '../../../models/employee.model';
import { EmployeeService } from '../../../services/employee.service';

@Component({
  selector: 'app-delete-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './delete-confirm-dialog.component.html',
  styleUrls: ['./delete-confirm-dialog.component.css']
})
export class DeleteModalComponent {
  @Input() employee: Employee = {} as Employee;
  @Output() deleted = new EventEmitter<number>();
  @Output() closed = new EventEmitter<void>();

  error: string | null = null;
  isLoading: boolean = false;

  constructor(private employeeService: EmployeeService) {}

  delete(): void {
    this.error = null;
    this.isLoading = true;

    this.employeeService.deleteEmployee(this.employee.employeeId).subscribe({
      next: () => {
        this.isLoading = false;
        this.deleted.emit(this.employee.employeeId);
        this.close();
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Error deleting employee:', error);
        
        if (error.error && typeof error.error === 'string') {
          this.error = error.error;
        } else {
          this.error = 'Failed to delete employee';
        }
      }
    });
  }

  close(): void {
    this.closed.emit();
  }
} 