﻿namespace Gradebook.Web.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using Data.Common.Models;
    using Data.Common.Repositories;
    using Data.Models;
    using Gradebook.Services.Data.Interfaces;
    using Gradebook.Services.Mapping;
    using Interfaces;
    using ViewModels.InputModels;

    public class StudentsService : IStudentsService
    {
        private readonly IDeletableEntityRepository<Student> _studentsRepository;
        private readonly IDeletableEntityRepository<School> _schoolsRepository;
        private readonly IIdGeneratorService _idGeneratorService;

        public StudentsService(IDeletableEntityRepository<Student> studentsRepository, IDeletableEntityRepository<School> schoolsRepository, IIdGeneratorService idGeneratorService)
        {
            _studentsRepository = studentsRepository;
            _schoolsRepository = schoolsRepository;
            _idGeneratorService = idGeneratorService;
        }

        public int? GetIdByUniqueId(string uniqueId)
        {
            var student = _studentsRepository.All().FirstOrDefault(s => s.UniqueId == uniqueId);
            return student?.Id;
        }

        public T GetById<T>(int id)
        {
            var student = _studentsRepository.All().Where(s => s.Id == id);
            return student.To<T>().FirstOrDefault();
        }

        public async Task EditAsync(StudentModifyInputModel modifiedModel)
        {
            var student = _studentsRepository.All().FirstOrDefault(s => s.Id == modifiedModel.Id);
            if (student != null)
            {
                var inputModel = modifiedModel.Student;
                student.FirstName = inputModel.FirstName;
                student.LastName = inputModel.LastName;
                student.BirthDate = inputModel.BirthDate;
                student.PersonalIdentificationNumber = inputModel.PersonalIdentificationNumber;

                var schoolId = int.Parse(inputModel.SchoolId);
                var school = _schoolsRepository.All().FirstOrDefault(s => s.Id == schoolId);
                if (school != null)
                {
                    student.School = school;
                    var classId = int.Parse(inputModel.ClassId);
                    if (school.Classes.Any(c => c.Id == classId))
                    {
                        student.Class = school.Classes.FirstOrDefault(c => c.Id == classId);
                    }
                }

                _studentsRepository.Update(student);
                await _studentsRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var student = _studentsRepository.All().FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _studentsRepository.Delete(student);
                await _studentsRepository.SaveChangesAsync();
            }
        }

        public async Task<T> CreateStudent<T>(StudentInputModel inputModel)
        {
            var schoolId = int.Parse(inputModel.SchoolId);
            var school = _schoolsRepository.All().FirstOrDefault(s => s.Id == schoolId);
            if (school != null)
            {
                var classId = int.Parse(inputModel.ClassId);
                if (school.Classes.Any(c => c.Id == classId))
                {
                    var student = new Student
                    {
                        FirstName = inputModel.FirstName,
                        LastName = inputModel.LastName,
                        BirthDate = inputModel.BirthDate,
                        PersonalIdentificationNumber = inputModel.PersonalIdentificationNumber,
                        School = school,
                        Class = school.Classes.FirstOrDefault(c => c.Id == classId),
                        UniqueId = _idGeneratorService.GenerateStudentId()
                    };

                    await _studentsRepository.AddAsync(student);
                    await _studentsRepository.SaveChangesAsync();

                    BasePersonModel baseModel = _studentsRepository.All().FirstOrDefault(p =>
                        p.FirstName == inputModel.FirstName && p.LastName == inputModel.LastName && p.PersonalIdentificationNumber == inputModel.PersonalIdentificationNumber); //ToDo: Just change to uniqueId from student above

                    return AutoMapperConfig.MapperInstance.Map<T>(baseModel);
                }

                throw new ArgumentException($"Sorry, school with id {schoolId} doesn't contain class with id {classId}");
            }

            throw new ArgumentException($"Sorry, we couldn't find school with id {schoolId}");
        }
    }
}