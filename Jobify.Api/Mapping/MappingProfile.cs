﻿using AutoMapper;
using Jobify.Api.DTOs;
using Jobify.BL.DALModels;

namespace Jobify.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Status, StatusDTO>().ReverseMap();
            CreateMap<UserType, UserTypeDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<Firm, FirmDTO>()
                //.ForMember(dest => dest.AverageGrade, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Firm, FirmSimplifiedDTO>()
                //.ForMember(dest => dest.AverageGrade, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<JobAd, JobAdDTO>().ReverseMap();
            CreateMap<Employer, EmployerDTO>().ReverseMap();            
            CreateMap<Employer, EmployerRegistrationDTO>().ReverseMap();            
            
            CreateMap<JobApp, JobAppDTO>().ReverseMap();
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Student, StudentRegistrationDTO>().ReverseMap();

            CreateMap<Admin, AdminDTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ReverseMap();
        }
    }
}
