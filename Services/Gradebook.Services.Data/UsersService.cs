namespace Gradebook.Services.Data
{
    using System.Linq;
    using Common;
    using Gradebook.Data.Common.Models;
    using Gradebook.Data.Common.Repositories;
    using Gradebook.Data.Models;
    using Interfaces;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<Principal> _principalsRepository;
        private readonly IDeletableEntityRepository<Teacher> _teachersRepository;
        private readonly IDeletableEntityRepository<Student> _studentsRepository;
        private readonly IDeletableEntityRepository<Parent> _parentsRepository;

        public UsersService(
            IDeletableEntityRepository<Principal> principalsRepository,
            IDeletableEntityRepository<Teacher> teachersRepository,
            IDeletableEntityRepository<Student> studentsRepository,
            IDeletableEntityRepository<Parent> parentsRepository)
        {
            _principalsRepository = principalsRepository;
            _teachersRepository = teachersRepository;
            _studentsRepository = studentsRepository;
            _parentsRepository = parentsRepository;
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
    }
}