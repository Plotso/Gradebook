﻿namespace Gradebook.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using Data.Common.Repositories;
    using Data.Models;
    using Gradebook.Services.Mapping;
    using Interfaces;
    using Microsoft.EntityFrameworkCore.Internal;
    using ViewModels.Principal;
    using ViewModels.Subject;

    public class SubjectsService : ISubjectsService
    {
        private readonly IDeletableEntityRepository<Subject> _subjectsRepository;
        private readonly IDeletableEntityRepository<StudentSubject> _studentSubjectRepository;
        private readonly IDeletableEntityRepository<Teacher> _teachersRepository;
        private readonly IDeletableEntityRepository<Class> _classesRepository;

        public SubjectsService(
            IDeletableEntityRepository<Subject> subjectsRepository,
            IDeletableEntityRepository<StudentSubject> studentSubjectRepository,
            IDeletableEntityRepository<Teacher> teachersRepository,
            IDeletableEntityRepository<Class> classesRepository)
        {
            _subjectsRepository = subjectsRepository;
            _studentSubjectRepository = studentSubjectRepository;
            _teachersRepository = teachersRepository;
            _classesRepository = classesRepository;
        }

        public IEnumerable<T> GetAllByTeacherId<T>(int teacherId)
        {
            var subjects = _subjectsRepository.All().Where(s => s.TeacherId == teacherId);
            return subjects.To<T>().ToList();
        }

        public IEnumerable<T> GetAllByStudentId<T>(int studentId)
        {
            var studentSubjects = _studentSubjectRepository.All().Where(s => s.StudentId == studentId);
            var subjects = studentSubjects.Select(s => s.Subject);
            return subjects.To<T>().ToList();
        }

        public IEnumerable<T> GetAllByMultipleStudentIds<T>(List<int> studentIds)
        {
            var studentSubjects = _studentSubjectRepository.All().Where(s => studentIds.Contains(s.StudentId));
            var subjects = studentSubjects.Select(s => s.Subject);
            return subjects.To<T>().ToList();
        }

        public IEnumerable<T> GetAllBySchoolId<T>(int schoolId)
        {
            var subjects = _subjectsRepository.All().Where(s => s.Teacher.SchoolId == schoolId);
            return subjects.To<T>().ToList();
        }

        public IEnumerable<T> GetAllByMultipleSchoolIds<T>(List<int> schoolIds)
        {
            var subjects = _subjectsRepository.All().Where(s => schoolIds.Contains(s.Teacher.SchoolId));
            return subjects.To<T>().ToList();
        }

        public IEnumerable<T> GetAll<T>()
        {
            var subjects = _subjectsRepository.All();
            return subjects.To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            var subject = _subjectsRepository.All().Where(s => s.Id == id);
            return subject.To<T>().FirstOrDefault();
        }

        public async Task CreateSubject(SubjectInputModel inputModel)
        {
            var teacherId = int.Parse(inputModel.TeacherId);
            var teacher = _teachersRepository.All().FirstOrDefault(t => t.Id == teacherId);
            if (teacher != null)
            {
                var subject = new Subject
                {
                    Name = inputModel.Name,
                    YearGrade = inputModel.YearGrade,
                    SchoolYear = inputModel.SchoolYear,
                    Teacher = teacher
                };

                await _subjectsRepository.AddAsync(subject);
                await _subjectsRepository.SaveChangesAsync();

                return;
            }

            throw new ArgumentException($"Sorry, we couldn't find teacher with id {teacherId}");
        }

        public async Task EditAsync(SubjectModifyInputModel modifiedModel)
        {
            var subject = _subjectsRepository.All().FirstOrDefault(s => s.Id == modifiedModel.Id);
            if (subject != null)
            {
                var inputModel = modifiedModel.Subject;
                subject.Name = inputModel.Name;
                subject.YearGrade = inputModel.YearGrade;
                subject.SchoolYear = inputModel.SchoolYear;

                var teacherId = int.Parse(inputModel.TeacherId);
                if (subject.TeacherId != teacherId)
                {
                    var teacher = _teachersRepository.All().FirstOrDefault(t => t.Id == teacherId);
                    if (teacher != null)
                    {
                        subject.Teacher = teacher;
                    }
                }

                _subjectsRepository.Update(subject);
                await _subjectsRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var subject = _subjectsRepository.All().FirstOrDefault(s => s.Id == id);
            if (subject != null)
            {
                if (subject.StudentSubjects.Any())
                {
                    var allStudentsForSubject = _studentSubjectRepository.All().Where(s => s.SubjectId == id);
                    foreach (var studentSubjectPair in allStudentsForSubject)
                    {
                        _studentSubjectRepository.Delete(studentSubjectPair);  // ToDo: Decide if grades/absences delete behaviour should be handled here as well
                    }

                    await _studentSubjectRepository.SaveChangesAsync();
                }

                _subjectsRepository.Delete(subject);
                await _subjectsRepository.SaveChangesAsync();
            }
        }

        public T GetStudentSubjectPair<T>(int studentId, int subjectId)
        {
            var studentSubjects = _studentSubjectRepository.All()
                .Where(s => s.StudentId == studentId && s.SubjectId == subjectId);
            return studentSubjects.To<T>().FirstOrDefault();
        }

        public async Task AssignAllStudentsFromClassToSubject(ClassSubjectInputModel inputModel)
        {
            var subject = _subjectsRepository.All().FirstOrDefault(s => s.Id == int.Parse(inputModel.SubjectId));
            if (subject != null)
            {
                var schoolClass = _classesRepository.All().FirstOrDefault(c => c.Id == int.Parse(inputModel.ClassId));
                if (schoolClass != null)
                {
                    if (schoolClass.Teacher.SchoolId != subject.Teacher.SchoolId)
                    {
                        throw new ArgumentException($"Sorry, class with id {inputModel.ClassId} and subject with id {inputModel.SubjectId} are in different schools");
                    }

                    if (schoolClass.Students.Any())
                    {
                        foreach (var student in schoolClass.Students)
                        {
                            var studentSubjectPair = new StudentSubject
                            {
                                Student = student,
                                Subject = subject
                            };

                            await _studentSubjectRepository.AddAsync(studentSubjectPair);
                        }

                        await _studentSubjectRepository.SaveChangesAsync();
                    }

                    return;
                }

                throw new ArgumentException($"Sorry, we couldn't find class with id {inputModel.ClassId}");
            }

            throw new ArgumentException($"Sorry, we couldn't find subject with id {inputModel.SubjectId}");
        }
    }
}