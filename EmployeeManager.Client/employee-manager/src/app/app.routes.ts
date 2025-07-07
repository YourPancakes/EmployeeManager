import { Routes } from '@angular/router';
import { AboutComponent } from './components/about/about.component';
import { EmployeesComponent } from './components/employees/employees-list/employees-list.component';

export const routes: Routes = [
  { path: '', component: AboutComponent },
  { path: 'employees', component: EmployeesComponent },
  { path: '**', redirectTo: '' }
];
