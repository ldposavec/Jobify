using AutoMapper;
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

            CreateMap<Firm, FirmDTO>().ReverseMap();
            CreateMap<JobAd, JobAdDTO>().ReverseMap();
            CreateMap<Employer, EmployerDTO>().ReverseMap();            
            
            CreateMap<JobApp, JobAppDTO>().ReverseMap();
            CreateMap<Student, StudentDTO>().ReverseMap();

            CreateMap<Admin, AdminDTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ReverseMap();
        }
    }
}
