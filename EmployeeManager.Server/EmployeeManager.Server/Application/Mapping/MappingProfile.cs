using AutoMapper;
using EmployeeManager.Server.Application.DTO;
using EmployeeManager.Server.Domain.Entities;

namespace EmployeeManager.Server.Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for mapping between domain entities and data transfer objects.
    /// Defines all object-to-object mappings used throughout the application.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the MappingProfile and configures all mappings.
        /// </summary>
        public MappingProfile()
        {
            ConfigureCompanyMappings();
            ConfigureEmployeeMappings();
            ConfigureDepartmentMappings();
        }

        /// <summary>
        /// Configures mappings for Company entity and related DTOs.
        /// </summary>
        private void ConfigureCompanyMappings()
        {
            CreateMap<Company, CompanyDto>();
        }

        /// <summary>
        /// Configures mappings for Employee entity and related DTOs.
        /// </summary>
        private void ConfigureEmployeeMappings()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(destination => destination.DepartmentName,
                    options => options.MapFrom(source => source.Department != null ? source.Department.Name : string.Empty));

            CreateMap<EmployeeCreateDto, Employee>();
            CreateMap<EmployeeUpdateDto, Employee>();
        }

        /// <summary>
        /// Configures mappings for Department entity and related DTOs.
        /// </summary>
        private void ConfigureDepartmentMappings()
        {
            CreateMap<Department, DepartmentDto>()
                .ForMember(destination => destination.CompanyName,
                    options => options.MapFrom(source => source.Company != null ? source.Company.Name : string.Empty));
            CreateMap<DepartmentCreateDto, Department>();
            CreateMap<DepartmentUpdateDto, Department>();
        }
    }
}