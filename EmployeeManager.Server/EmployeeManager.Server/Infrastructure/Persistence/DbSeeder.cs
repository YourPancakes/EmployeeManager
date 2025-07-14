using EmployeeManager.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Server.Infrastructure.Persistence
{
    /// <summary>
    /// Database seeder for initial data population.
    /// Provides methods to seed the database with sample departments and employees for development and testing.
    /// </summary>
    public static class DbSeeder
    {
        /// <summary>
        /// Seeds the database with initial data including departments and employees.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency injection</param>
        /// <returns>A task representing the asynchronous seeding operation</returns>
        /// <exception cref="InvalidOperationException">Thrown when departments are not available for employee seeding</exception>
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EmployeeManagerDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<EmployeeManagerDbContext>>();

            try
            {
                logger.LogInformation("Starting database seeding process...");

                await SeedCompaniesIfNeededAsync(context, logger);
                await SeedDepartmentsIfNeededAsync(context, logger);
                await SeedEmployeesIfNeededAsync(context, logger);

                logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred while seeding the database");
                throw;
            }
        }

        private static async Task SeedCompaniesIfNeededAsync(EmployeeManagerDbContext context, ILogger logger)
        {
            try
            {
                if (!await context.Companies.AnyAsync())
                {
                    logger.LogInformation("No companies found, seeding companies...");
                    await SeedCompaniesAsync(context);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Companies seeded successfully");
                }
                else
                {
                    var companyCount = await context.Companies.CountAsync();
                    logger.LogInformation("Companies already exist ({CompanyCount} companies), skipping company seeding", companyCount);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding companies");
                throw;
            }
        }

        private static async Task SeedDepartmentsIfNeededAsync(EmployeeManagerDbContext context, ILogger logger)
        {
            try
            {
                if (!await context.Departments.AnyAsync())
                {
                    logger.LogInformation("No departments found, seeding departments...");

                    var company = await context.Companies.FirstOrDefaultAsync();
                    if (company == null)
                    {
                        throw new InvalidOperationException("No company found. Companies must be seeded before departments.");
                    }

                    logger.LogInformation("Found company: {CompanyName} (ID: {CompanyId})", company.Name, company.CompanyId);

                    await SeedDepartmentsAsync(context);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Departments seeded successfully");
                }
                else
                {
                    var departmentCount = await context.Departments.CountAsync();
                    logger.LogInformation("Departments already exist ({DepartmentCount} departments), skipping department seeding", departmentCount);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding departments");
                throw;
            }
        }

        private static async Task SeedEmployeesIfNeededAsync(EmployeeManagerDbContext context, ILogger logger)
        {
            try
            {
                if (!await context.Employees.AnyAsync())
                {
                    logger.LogInformation("No employees found, seeding employees...");

                    var departments = await context.Departments.ToListAsync();
                    if (!departments.Any())
                    {
                        throw new InvalidOperationException("Departments must be seeded before employees");
                    }

                    logger.LogInformation("Found {DepartmentCount} departments for employee seeding", departments.Count);

                    await SeedEmployeesAsync(context);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Employees seeded successfully");
                }
                else
                {
                    var employeeCount = await context.Employees.CountAsync();
                    logger.LogInformation("Employees already exist ({EmployeeCount} employees), skipping employee seeding", employeeCount);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding employees");
                throw;
            }
        }

        private static async Task SeedCompaniesAsync(EmployeeManagerDbContext context)
        {
            var companies = new List<Company>
            {
                new Company
                {
                    Name = "Employee Manager Corp",
                    Founded = 2024,
                    Industry = "Software Development",
                    Description = "Leading provider of employee management solutions",
                    Headquarters = "Tech City, Innovation State",
                    Website = "https://employeemanager.com"
                }
            };

            await context.Companies.AddRangeAsync(companies);
        }

        private static async Task SeedDepartmentsAsync(EmployeeManagerDbContext context)
        {
            var company = await context.Companies.FirstOrDefaultAsync();

            if (company == null)
            {
                throw new InvalidOperationException("No company found. Companies must be seeded before departments.");
            }

            var departments = new List<Department>
            {
                new Department { CompanyId = company.CompanyId, Name = "IT Department" },
                new Department { CompanyId = company.CompanyId, Name = "HR Department" },
                new Department { CompanyId = company.CompanyId, Name = "Finance Department" },
                new Department { CompanyId = company.CompanyId, Name = "Marketing Department" },
                new Department { CompanyId = company.CompanyId, Name = "Operations Department" },
                new Department { CompanyId = company.CompanyId, Name = "Research & Development" },
                new Department { CompanyId = company.CompanyId, Name = "Customer Support" },
                new Department { CompanyId = company.CompanyId, Name = "Legal Department" }
            };

            await context.Departments.AddRangeAsync(departments);
        }

        private static async Task SeedEmployeesAsync(EmployeeManagerDbContext context)
        {
            var departments = await context.Departments.ToListAsync();

            if (!departments.Any())
            {
                throw new InvalidOperationException("Departments must be seeded before employees");
            }

            var employees = new List<Employee>
            {
                new Employee
                {
                    DepartmentId = departments.First(department => department.Name == "IT Department").DepartmentId,
                    FullName = "John Smith",
                    BirthDate = new DateTime(1985, 3, 15),
                    HireDate = new DateTime(2020, 1, 15),
                    Salary = 75000
                },
                new Employee
                {
                    DepartmentId = departments.First(department => department.Name == "IT Department").DepartmentId,
                    FullName = "Sarah Johnson",
                    BirthDate = new DateTime(1990, 7, 22),
                    HireDate = new DateTime(2021, 3, 10),
                    Salary = 82000
                },
                new Employee
                {
                    DepartmentId = departments.First(department => department.Name == "HR Department").DepartmentId,
                    FullName = "Michael Brown",
                    BirthDate = new DateTime(1988, 11, 8),
                    HireDate = new DateTime(2019, 6, 20),
                    Salary = 65000
                },
                new Employee
                {
                    DepartmentId = departments.First(department => department.Name == "Finance Department").DepartmentId,
                    FullName = "Emily Davis",
                    BirthDate = new DateTime(1992, 4, 12),
                    HireDate = new DateTime(2022, 1, 5),
                    Salary = 70000
                },
                new Employee
                {
                    DepartmentId = departments.First(department => department.Name == "Marketing Department").DepartmentId,
                    FullName = "David Wilson",
                    BirthDate = new DateTime(1987, 9, 30),
                    HireDate = new DateTime(2020, 8, 15),
                    Salary = 68000
                },
                new Employee
                {
                    DepartmentId = departments.First(department => department.Name == "Operations Department").DepartmentId,
                    FullName = "Lisa Anderson",
                    BirthDate = new DateTime(1991, 12, 3),
                    HireDate = new DateTime(2021, 11, 1),
                    Salary = 62000
                },
                new Employee
                {
                    DepartmentId = departments.First(department => department.Name == "Research & Development").DepartmentId,
                    FullName = "Robert Taylor",
                    BirthDate = new DateTime(1986, 5, 18),
                    HireDate = new DateTime(2018, 4, 12),
                    Salary = 90000
                },
                new Employee
                {
                    DepartmentId = departments.First(department => department.Name == "Customer Support").DepartmentId,
                    FullName = "Jennifer Garcia",
                    BirthDate = new DateTime(1993, 8, 25),
                    HireDate = new DateTime(2022, 2, 14),
                    Salary = 55000
                },
                new Employee
                {
                    DepartmentId = departments.First(department => department.Name == "Legal Department").DepartmentId,
                    FullName = "Christopher Martinez",
                    BirthDate = new DateTime(1984, 1, 7),
                    HireDate = new DateTime(2019, 9, 3),
                    Salary = 85000
                },
                new Employee
                {
                    DepartmentId = departments.First(department => department.Name == "IT Department").DepartmentId,
                    FullName = "Amanda Rodriguez",
                    BirthDate = new DateTime(1989, 6, 14),
                    HireDate = new DateTime(2021, 7, 8),
                    Salary = 78000
                }
            };

            await context.Employees.AddRangeAsync(employees);
        }
    }
}