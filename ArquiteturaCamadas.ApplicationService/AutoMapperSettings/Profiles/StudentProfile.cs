using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Student;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Student;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using AutoMapper;

namespace ArquiteturaCamadas.ApplicationService.AutoMapperSettings.Profiles
{
    public sealed class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<StudentSaveRequest, Student>()
                .ForMember(s => s.Address, map => map.Ignore());

            CreateMap<StudentUpdateRequest, Student>()
                .ForMember(s => s.Address, map => map.Ignore());

            CreateMap<Student, StudentResponse>();

            CreateMap<PageList<Student>, PageList<StudentResponse>>();
        }
    }
}
