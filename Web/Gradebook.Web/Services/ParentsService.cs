namespace Gradebook.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using Data.Common.Models;
    using Gradebook.Data.Common.Repositories;
    using Data.Models;
    using Gradebook.Services.Data.Interfaces;
    using Gradebook.Services.Mapping;
    using Interfaces;
    using Microsoft.EntityFrameworkCore.Internal;
    using ViewModels.InputModels;

    public class ParentsService : IParentsService
    {
        private readonly IDeletableEntityRepository<Parent> _parentsRepository;
        private readonly IRepository<Student> _studentsRepository;
        private readonly IDeletableEntityRepository<StudentParent> _studentParentsMappingRepository;
        private readonly IIdGeneratorService _idGeneratorService;

        public ParentsService(
            IDeletableEntityRepository<Parent> parentsRepository,
            IRepository<Student> studentsRepository, 
            IDeletableEntityRepository<StudentParent> studentParentsMappingRepository, 
            IIdGeneratorService idGeneratorService)
        {
            _parentsRepository = parentsRepository;
            _studentsRepository = studentsRepository;
            _studentParentsMappingRepository = studentParentsMappingRepository;
            _idGeneratorService = idGeneratorService;
        }

        public async Task<T> CreateParentAsync<T>(ParentInputModel inputModel)
        {
            var studentIds = inputModel.StudentIds.Select(int.Parse);
            var students = _studentsRepository.All().Where(s => studentIds.Contains(s.Id));
            if (students.Any())
            {
                var parent = new Parent
                {
                    FirstName = inputModel.FirstName,
                    LastName = inputModel.LastName,
                    PhoneNumber = inputModel.PhoneNumber,
                    UniqueId = _idGeneratorService.GenerateParentId()
                };

                await _parentsRepository.AddAsync(parent);
                await _parentsRepository.SaveChangesAsync();

                foreach (var student in students)
                {
                    var studentParentPair = new StudentParent
                    {
                        Student = student,
                        Parent = parent
                    };

                    await _studentParentsMappingRepository.AddAsync(studentParentPair);
                }

                await _studentParentsMappingRepository.SaveChangesAsync();

                BasePersonModel baseModel = _parentsRepository.All().FirstOrDefault(p => p.UniqueId == parent.UniqueId);

                return AutoMapperConfig.MapperInstance.Map<T>(baseModel);
            }

            throw new ArgumentException($"Sorry, we couldn't any student with id in the following list:  [{string.Join(", ", studentIds)}]");
        }

        public List<int> GetStudentIdsByParentUniqueId(string uniqueId)
        {
            var parent = _parentsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
            if (parent != null)
            {
                return parent.StudentParents.Select(s => s.StudentId).ToList();
            }

            return new List<int>();
        }

        public T GetById<T>(int id)
        {
            var parents = _parentsRepository.All().Where(p => p.Id == id);
            return parents.To<T>().FirstOrDefault();
        }

        public async Task EditAsync(ParentModifyInputModel modifiedModel)
        {
            var parent = _parentsRepository.All().FirstOrDefault(p => p.Id == modifiedModel.Id);
            if (parent != null)
            {
                var studentIds = modifiedModel.Parent.StudentIds.Select(int.Parse).ToList();

                if (!studentIds.Any()) // Make sure parent has student children
                {
                    throw new ArgumentException($"Sorry, it's mandatory for a parent user to have at least 1 student");
                }

                if (HasDifferentStudentIds(parent, studentIds) && studentIds.Any())
                {
                    var students = _studentsRepository.All().Where(s => studentIds.Contains(s.Id));
                    // Remove all pairs that are no longer valid
                    foreach (var studentParent in parent.StudentParents.Where(sp => !studentIds.Contains(sp.StudentId)))
                    {
                        _studentParentsMappingRepository.Delete(studentParent);
                    }

                    foreach (var studentId in studentIds.Where(sid => !parent.StudentParents.Select(sp => sp.StudentId).Contains(sid)))
                    {
                        var student = _studentsRepository.All().FirstOrDefault(s => s.Id == studentId);
                        if (student != null)
                        {
                            var studentParentPair = new StudentParent
                            {
                                Student = student,
                                Parent = parent
                            };

                            await _studentParentsMappingRepository.AddAsync(studentParentPair);
                        }
                    }

                    await _studentParentsMappingRepository.SaveChangesAsync();
                }

                parent.FirstName = modifiedModel.Parent.FirstName;
                parent.LastName = modifiedModel.Parent.LastName;
                parent.PhoneNumber = modifiedModel.Parent.PhoneNumber;

                _parentsRepository.Update(parent);
                await _parentsRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var parent = _parentsRepository.All().FirstOrDefault(p => p.Id == id);
            if (parent != null)
            {
                foreach (var studentPair in parent.StudentParents)
                {
                    _studentParentsMappingRepository.Delete(studentPair);
                }

                await _studentParentsMappingRepository.SaveChangesAsync();
                
                _parentsRepository.Delete(parent);
                await _parentsRepository.SaveChangesAsync();
            }
        }

        private bool HasDifferentStudentIds(Parent parent, IList<int> studentIds)
        {
            var parentStudents = parent.StudentParents;
            if (parentStudents.Count == studentIds.Count())
            {
                var parentStudentIds = parentStudents.Select(s => s.StudentId);
                if (studentIds.All(sid => parentStudentIds.Contains(sid)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}