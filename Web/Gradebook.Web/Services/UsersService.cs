namespace Gradebook.Web.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Data.Common.Models;
    using Data.Common.Repositories;
    using Data.Models;
    using Gradebook.Services.Data;
    using Gradebook.Services.Data.Interfaces;
    using Gradebook.Services.Mapping;
    using Interfaces;
    using ViewModels.InputModels;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<Principal> _principalsRepository;
        private readonly IDeletableEntityRepository<Teacher> _teachersRepository;
        private readonly IDeletableEntityRepository<Student> _studentsRepository;
        private readonly IDeletableEntityRepository<Parent> _parentsRepository;
        private readonly IIdGeneratorService _idGeneratorService;

        public UsersService(
            IDeletableEntityRepository<Principal> principalsRepository,
            IDeletableEntityRepository<Teacher> teachersRepository,
            IDeletableEntityRepository<Student> studentsRepository,
            IDeletableEntityRepository<Parent> parentsRepository,
            IIdGeneratorService idGeneratorService)
        {
            _principalsRepository = principalsRepository;
            _teachersRepository = teachersRepository;
            _studentsRepository = studentsRepository;
            _parentsRepository = parentsRepository;
            _idGeneratorService = idGeneratorService;
        }

        public async Task<T> CreatePrincipal<T>(PrincipalInputModel inputModel)
        {
            var principal = new Principal
            {
                FirstName = inputModel.FirstName,
                LastName = inputModel.LastName,
                UniqueId = _idGeneratorService.GeneratePrincipalId()
            };

            await _principalsRepository.AddAsync(principal);
            await _principalsRepository.SaveChangesAsync();
            BasePersonModel baseModel = _principalsRepository.All().FirstOrDefault(p =>
                p.FirstName == inputModel.FirstName && p.LastName == inputModel.LastName);

            return AutoMapperConfig.MapperInstance.Map<T>(baseModel);
        }

        public UserType GetUserTypeByUniqueId(string uniqueId)
        {
            if (!string.IsNullOrEmpty(uniqueId))
            {
                switch (uniqueId[0])
                {
                    case GlobalConstants.PrincipalIdPrefix:
                        var principalRecord = _principalsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        if (principalRecord != null)
                        {
                            return UserType.Principal;
                        }

                        break;
                    case GlobalConstants.TeacherIdPrefix:
                        var teacherRecord = _teachersRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        if (teacherRecord != null)
                        {
                            return UserType.Teacher;
                        }

                        break;
                    case GlobalConstants.StudentIdPrefix:
                        var studentRecord = _studentsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        if (studentRecord != null)
                        {
                            return UserType.Student;
                        }

                        break;
                    case GlobalConstants.ParentIdPrefix:
                        var parentRecord = _parentsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        if (parentRecord != null)
                        {
                            return UserType.Parent;
                        }

                        break;
                }
            }

            return UserType.None;
        }

        public Principal GetPrincipalByUniqueId(string uniqueId)
        {
            var principalRecord = _principalsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
            return principalRecord;
        }

        public async Task SetUserEmail(string uniqueId, string email)
        {
            if (!string.IsNullOrEmpty(uniqueId))
            {
                switch (uniqueId[0])
                {
                    case GlobalConstants.TeacherIdPrefix:
                        var teacherRecord = _teachersRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        teacherRecord.Email = email;
                        _teachersRepository.Update(teacherRecord);
                        await _teachersRepository.SaveChangesAsync();

                        break;
                    case GlobalConstants.StudentIdPrefix:
                        var studentRecord = _studentsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        studentRecord.Email = email;
                        studentRecord.Username = email;
                        _studentsRepository.Update(studentRecord);
                        await _studentsRepository.SaveChangesAsync();

                        break;
                    case GlobalConstants.ParentIdPrefix:
                        var parentRecord = _parentsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        parentRecord.Email = email;
                        _parentsRepository.Update(parentRecord);
                        await _parentsRepository.SaveChangesAsync();

                        break;
                }
            }
        }

        public IEnumerable<School> GetUserSchoolsByUniqueId(string uniqueId)
        {
            if (!string.IsNullOrEmpty(uniqueId))
            {
                switch (uniqueId[0])
                {
                    case GlobalConstants.PrincipalIdPrefix:
                        var principalRecord = _principalsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        if (principalRecord != null)
                        {
                            return new[] { principalRecord.School };
                        }

                        break;
                    case GlobalConstants.TeacherIdPrefix:
                        var teacherRecord = _teachersRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        if (teacherRecord != null)
                        {
                            return new[] { teacherRecord.School };
                        }

                        break;
                    case GlobalConstants.StudentIdPrefix:
                        var studentRecord = _studentsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        if (studentRecord != null)
                        {
                            return new[] { studentRecord.School };
                        }

                        break;
                    case GlobalConstants.ParentIdPrefix:
                        var parentRecord = _parentsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
                        if (parentRecord != null)
                        {
                            return parentRecord.StudentParents.Select(s => s.Student.School);
                        }

                        break;
                }
            }

            return null;
        }
    }
}