using System;
using WebApiTest.Api.Dto;
using WebApiTest.Api.Entities;

namespace WebApiTest.Api.Profile
{
    public class EmployeeProfile : AutoMapper.Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.GenderDisplay,
                    opt => opt.MapFrom(src => src.Gender.ToString())
                )
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => DateTime.Now.Year - src.DateOfBirth.Year)
                );
            CreateMap<EmployeeAddDto, Employee>();
            CreateMap<Employee, EmployeeAddDto>();

        }
    }
}