# Employee Manager

## Overview

Employee Manager is a full-stack application for managing employees, departments, and companies. It includes:
- **Frontend**: Angular (port 4200)
- **Backend**: ASP.NET Core Web API (port 5000)
- **Database**: SQL Server 2022 (port 1433)

## Quick Start (Docker Compose)

### Prerequisites
- Docker Desktop
- Docker Compose

### Build and Run
```bash
docker-compose up --build
```

- Frontend: http://localhost:4200
- Backend API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

### Stopping
```bash
docker-compose down
```

## API Usage Examples (cURL)

### 1. Get all employees
```bash
curl -X GET "http://localhost:5000/api/v1/employees" -H "accept: application/json"
```

### 2. Get employees with salary above 10000
```bash
curl -X GET "http://localhost:5000/api/v1/employees/salary-above/10000" -H "accept: application/json"
```

### 3. Delete all employees older than 70
```bash
curl -X DELETE "http://localhost:5000/api/v1/employees/older-than/70" -H "accept: application/json"
```

### 4. Update salary to 15000 for those earning less
```bash
curl -X PUT "http://localhost:5000/api/v1/employees/update-salary?newSalary=15000&maximumCurrentSalary=14999" -H "accept: application/json"
```

## SQL Query Examples

### 1.  Get all employees
```sql
SELECT * FROM Employees;
```

### 2. Get employees with salary above 10000
```sql
SELECT * FROM Employees WHERE Salary > 10000;
```

### 3. Delete all employees older than 70
```sql
DELETE FROM Employees WHERE DATEDIFF(YEAR, BirthDate, GETDATE()) > 70;
```

### 4. Update salary to 15000 for those earning less
```sql
UPDATE Employees SET Salary = 15000 WHERE Salary < 15000;
```