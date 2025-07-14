using EmployeeManager.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Server.Infrastructure.Persistence
{
    public class EmployeeManagerDbContext : DbContext
    {
        private const int MAXIMUM_DEPARTMENT_NAME_LENGTH = 100;
        private const int MAXIMUM_EMPLOYEE_NAME_LENGTH = 200;
        private const string SALARY_COLUMN_TYPE = "decimal(18,2)";

        public EmployeeManagerDbContext(DbContextOptions<EmployeeManagerDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureCompanyEntity(modelBuilder);
            ConfigureDepartmentEntity(modelBuilder);
            ConfigureEmployeeEntity(modelBuilder);
        }

        private static void ConfigureCompanyEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(company => company.CompanyId);

                entity.Property(company => company.CompanyId).ValueGeneratedOnAdd();

                entity.Property(company => company.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(company => company.Founded)
                    .IsRequired();

                entity.Property(company => company.Industry)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(company => company.Description)
                    .HasMaxLength(500);

                entity.Property(company => company.Headquarters)
                    .HasMaxLength(200);

                entity.Property(company => company.Website)
                    .HasMaxLength(200);
            });
        }

        private static void ConfigureDepartmentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(department => department.DepartmentId);

                entity.Property(department => department.DepartmentId).ValueGeneratedOnAdd();

                entity.Property(department => department.Name)
                    .IsRequired()
                    .HasMaxLength(MAXIMUM_DEPARTMENT_NAME_LENGTH);

                entity.HasOne(department => department.Company)
                    .WithMany(company => company.Departments)
                    .HasForeignKey(department => department.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(department => new { department.CompanyId, department.Name }).IsUnique();
            });
        }

        private static void ConfigureEmployeeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(employee => employee.EmployeeId);

                entity.Property(employee => employee.EmployeeId).ValueGeneratedOnAdd();

                entity.Property(employee => employee.FullName)
                    .IsRequired()
                    .HasMaxLength(MAXIMUM_EMPLOYEE_NAME_LENGTH);

                entity.Property(employee => employee.BirthDate)
                    .IsRequired();

                entity.Property(employee => employee.HireDate)
                    .IsRequired();

                entity.Property(employee => employee.Salary)
                    .IsRequired()
                    .HasColumnType(SALARY_COLUMN_TYPE);

                entity.HasOne(employee => employee.Department)
                    .WithMany(department => department.Employees)
                    .HasForeignKey(employee => employee.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}