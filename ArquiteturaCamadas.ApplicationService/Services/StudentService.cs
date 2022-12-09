using ArquiteturaCamadas.ApplicationService.AutoMapperSettings;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Arguments;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Student;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Responses.Student;
using ArquiteturaCamadas.ApplicationService.Interfaces;
using ArquiteturaCamadas.ApplicationService.Services.ServiceBase;
using ArquiteturaCamadas.Business.Extensions;
using ArquiteturaCamadas.Business.Interfaces.Notification;
using ArquiteturaCamadas.Business.Interfaces.Repositories;
using ArquiteturaCamadas.Business.Settings.PaginationSettings;
using ArquiteturaCamadas.Domain.Entities;
using ArquiteturaCamadas.Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ArquiteturaCamadas.ApplicationService.Services
{
    public sealed class StudentService : BaseService<Student>, IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICepService _cepService;

        public StudentService(IStudentRepository studentRepository, ICepService cepService,
                              IValidator<Student> validator, INotificationHandler notification) 
                              : base(validator, notification)
        {
            _studentRepository = studentRepository;
            _cepService = cepService;
        }

        public async Task<bool> AddAsync(StudentSaveRequest studentSaveRequest)
        {
            var address = await _cepService.GetAddressFromCepAsync(studentSaveRequest.Address.ZipCode);

            if (address is null)
                return false;

            address.Number = studentSaveRequest.Address.Number;

            if (address.Complement is not null)
                address.Complement = studentSaveRequest.Address.Complement;

            var student = studentSaveRequest.MapTo<StudentSaveRequest, Student>();
            student.Address = address;

            if (!await ValidateAsync(student))
                return false;

            return await _studentRepository.AddAsync(student);
        }

        public async Task<bool> UpdateAsync(StudentUpdateRequest studentUpdateRequest)
        {
            var student = await _studentRepository.FindByIdAsync(studentUpdateRequest.Id, s => s.Include(s => s.Address), true);

            if (student is null)
                return _notification.AddDomainNotification(EMessage.NotFound.ToString(), EMessage.NotFound.Description().FormatTo("Student"));

            var address = await _cepService.GetAddressFromCepAsync(studentUpdateRequest.Address.ZipCode);

            if (address is null)
                return false;

            address.Id = student.Address.Id;
            address.Number = studentUpdateRequest.Address.Number;

            if (address.Complement is not null)
                address.Complement = studentUpdateRequest.Address.Complement;

            student = studentUpdateRequest.MapTo<StudentUpdateRequest, Student>();
            
            if (!await ValidateAsync(student))
                return false;

            student.Address = address;
            student.Projects = new List<Project>();

            return await _studentRepository.UpdateAsync(student);
        }

        public async Task<bool> RemoveProjectAsync(ProjectStudentRelationShipArgument projectStudentRelationShipArgument)
        {
            var student = await _studentRepository.FindByIdAsync(projectStudentRelationShipArgument.StudentId, s => s.Include(s => s.Projects), false);

            if (student is null)
                return _notification.AddDomainNotification(EMessage.NotFound.ToString(), EMessage.NotFound.Description().FormatTo("Student"));

            var project = student.Projects.FirstOrDefault(p => p.Id == projectStudentRelationShipArgument.ProjectId);

            if (project is null)
                return _notification.AddDomainNotification(EMessage.NotFound.ToString(), EMessage.NotFound.Description().FormatTo("Project"));

            student.Projects.Remove(project);

            return await _studentRepository.UpdateRelationshipAsync(student);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if(!await _studentRepository.HaveObjectInDbAsync(s => s.Id == id))
                return _notification.AddDomainNotification(EMessage.NotFound.ToString(), EMessage.NotFound.Description().FormatTo("Student"));

            return await _studentRepository.DeleteStudentAsync(id);
        }

        public async Task<StudentResponse> FindByIdAsync(int id)
        {
            var student = await _studentRepository.FindByIdAsync(id, s => s.Include(s => s.Address).Include(s => s.Projects), true);

            return student.MapTo<Student, StudentResponse>();
        }

        public async Task<List<StudentResponse>> FindAllEntitiesAsync()
        {
            var studentsList = await _studentRepository.FindAllEntitiesAsync(s => s.Include(s => s.Address).Include(s => s.Projects));

            return studentsList.MapTo<List<Student>, List<StudentResponse>>();
        }

        public async Task<PageList<StudentResponse>> FindAllEntitiesWithPaginationAsync(PageParams pageParams)
        {
            var studentsPageList = await _studentRepository.FindAllEntitiesWithPaginationAsync(pageParams, s => s.Include(s => s.Address).Include(s => s.Projects));

            return studentsPageList.MapTo<PageList<Student>, PageList<StudentResponse>>();
        }

        public async Task<bool> HaveStudentInDbAsync(int id) =>
            await _studentRepository.HaveObjectInDbAsync(s => s.Id == id);
    }
}
