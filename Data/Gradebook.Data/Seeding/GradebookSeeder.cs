namespace Gradebook.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Services.Data;
    using Services.Data.Interfaces;

    public class GradebookSeeder : ISeeder
    {
        private IIdGeneratorService _idGeneratoService;

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            _idGeneratoService = serviceProvider.GetRequiredService<IIdGeneratorService>();

            await SeedPrincipals(dbContext);
            await SeedSchools(dbContext);
            await SeedTeachers(dbContext);
            await SeedClasses(dbContext);
            await SeedStudents(dbContext);
            await SeedSubjects(dbContext);
        }

        private async Task SeedPrincipals(ApplicationDbContext dbContext)
        {
            if (dbContext.Principals.Any())
            {
                return;
            }

            var principals = new List<Principal>()
            {
                new Principal { FirstName = "Emilia", LastName = "Emilianova", UniqueId = _idGeneratoService.GeneratePrincipalId()},
                new Principal { FirstName = "Emil", LastName = "Emilianov", UniqueId = _idGeneratoService.GeneratePrincipalId()},
                new Principal { FirstName = "Georgi", LastName = "georgiev", UniqueId = _idGeneratoService.GeneratePrincipalId()},
                new Principal { FirstName = "Toshko", LastName = "Toshkov", UniqueId = _idGeneratoService.GeneratePrincipalId()}
            };

            foreach (var principal in principals)
            {
                await dbContext.AddAsync(principal);
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task SeedSchools(ApplicationDbContext dbContext)
        {
            if (dbContext.Schools.Any())
            {
                return;
            }

            var schools = new List<School>
            {
                new School {Name = "Second English Language High School \"Thomas Jefferson\"", Address = "ul. 'Trayanova vrata' 26, 1408 g.k. Strelbishte, Sofia", Type = SchoolType.HighSchool},
                new School {Name = "National Trade and Banking High School", Address = "bul. 'Vitosha' 91, 1408 Ivan Vazov, Sofia", Type = SchoolType.HighSchool},
                new School {Name = "142 OU \"Veselin Hanchev\"", Address = "ul. 'Pchela' 21, 1618 g.k. Krasno selo, Sofia", Type = SchoolType.PrimarySchool},
                new School {Name = "New Bulgarian University (NBU)", Address = "ul. 'Montevideo' 21, 1618 g.k. Ovcha kupel 2, Sofia", Type = SchoolType.University}
            };

            var counter = 0;
            foreach (var school in schools)
            {
                var principal = await dbContext.Principals.Select(p => p).Skip(counter++).FirstOrDefaultAsync();
                if (principal != null)
                {
                    school.Principal = principal;
                }
                await dbContext.AddAsync(school);
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task SeedTeachers(ApplicationDbContext dbContext)
        {
            if (dbContext.Teachers.Any())
            {
                return;
            }

            var secondEnglishLanguageSchoolTeachers = new List<Teacher>
            {
                new Teacher{FirstName = "Yordan", LastName = "Apostolov", Email = "y.apostolov@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Petko", LastName = "Petkov", Email = "petko.p@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Ivanka", LastName = "Ivanova", Email = "iIvanova@gmail.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Mista", LastName = "Inglishmen", Email = "inglishMista@gmail.bg", UniqueId = _idGeneratoService.GenerateTeacherId()}
            };

            var nationalTradeAndBankingSchoolTeachers = new List<Teacher>
            {
                new Teacher{FirstName = "Ivan", LastName = "Ivanov", Email = "ivan-na-kvadrat@mail.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Radoi", LastName = "Bojinov", Email = "rboji@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Yavor", LastName = "Yavorov", Email = "yaya@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Natko", LastName = "Bankera", Email = "bankata@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()}
            };

            var primarySchoolTeachers = new List<Teacher>
            {
                new Teacher{FirstName = "Dobrina", LastName = "Loshotiikova", Email = "loshataDobri@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Agnes", LastName = "Matematichkata", Email = "a.math@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Bulgarkata", LastName = "Damqnova", Email = "bgD@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Gospoja", LastName = "Geografiq", Email = "geography@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()}
            };

            var newBulgarianUniversityTeachers = new List<Teacher>
            {
                new Teacher{FirstName = "Fizichkata", LastName = "Slavkova", Email = "physics_slavkova@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Himikut", LastName = "Mitko", Email = "mitko_has_chemistry@abv.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Zlobnata", LastName = "Psiholojka", Email = "psilogiqta_e_lesna_nauka@mail.bg", UniqueId = _idGeneratoService.GenerateTeacherId()},
                new Teacher{FirstName = "Finalniqt", LastName = "Gospodin", Email = "the_last_one@nbu.bg", UniqueId = _idGeneratoService.GenerateTeacherId()}
            };

            var teachers = new List<Teacher>();
            teachers.AddRange(secondEnglishLanguageSchoolTeachers);
            teachers.AddRange(nationalTradeAndBankingSchoolTeachers);
            teachers.AddRange(primarySchoolTeachers);
            teachers.AddRange(newBulgarianUniversityTeachers);

            foreach (var teacher in teachers)
            {
                await dbContext.AddAsync(teacher);
            }

            var primarySchool = await dbContext.Schools.FirstOrDefaultAsync(s => s.Type == SchoolType.PrimarySchool);
            if (primarySchool != null)
            {
                primarySchool.Teachers = primarySchoolTeachers;
                dbContext.Update(primarySchool);
            }

            var secondEnglishLanguageSchool = await dbContext.Schools.FirstOrDefaultAsync(s => s.Type == SchoolType.HighSchool && s.Name.Contains("Second English Language High School"));
            if (secondEnglishLanguageSchool != null)
            {
                secondEnglishLanguageSchool.Teachers = secondEnglishLanguageSchoolTeachers;
                dbContext.Update(secondEnglishLanguageSchool);
            }

            var ntbg = await dbContext.Schools.FirstOrDefaultAsync(s => s.Type == SchoolType.HighSchool && s.Name.Contains("National Trade and Banking High School"));
            if (ntbg != null)
            {
                ntbg.Teachers = nationalTradeAndBankingSchoolTeachers;
                dbContext.Update(ntbg);
            }

            var nbu = await dbContext.Schools.FirstOrDefaultAsync(s => s.Type == SchoolType.University && s.Name.Contains("New Bulgarian University"));
            if (nbu != null)
            {
                nbu.Teachers = newBulgarianUniversityTeachers;
                dbContext.Update(nbu);
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task SeedSubjects(ApplicationDbContext dbContext)
        {
            if (dbContext.Subjects.Any())
            {
                return;
            }

            var subjects5thGradePrimarySchool = new List<Subject>
            {
                new Subject { Name = "Geography", SchoolYear = "2020-2021", YearGrade = 5, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "geography@abv.bg")}
            };

            var subjects7thGradePrimarySchool = new List<Subject>
            {
                new Subject { Name = "Maths", SchoolYear = "2020-2021", YearGrade = 7, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "a.math@abv.bg")},
                new Subject { Name = "Bulgarian", SchoolYear = "2020-2021", YearGrade = 7, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "bgD@abv.bg")},
                new Subject { Name = "Geography", SchoolYear = "2020-2021", YearGrade = 7, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "geography@abv.bg")},
                new Subject { Name = "Chemistry", SchoolYear = "2020-2021", YearGrade = 7, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "loshataDobri@abv.bg")},
            };

            var subjects8thGrade2ELS = new List<Subject>
            {
                new Subject { Name = "Arts", SchoolYear = "2020-2021", YearGrade = 8, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "y.apostolov@abv.bg")},
                new Subject { Name = "English", SchoolYear = "2020-2021", YearGrade = 8, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "inglishMista@gmail.bg")},
                new Subject { Name = "Maths", SchoolYear = "2020-2021", YearGrade = 8, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "iIvanova@gmail.bg")},
                new Subject { Name = "Bulgarian", SchoolYear = "2020-2021", YearGrade = 8, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "petko.p@abv.bg")},
            };

            var subjects12thGrade2ELS = new List<Subject>
            {
                new Subject { Name = "Arts", SchoolYear = "2020-2021", YearGrade = 12, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "y.apostolov@abv.bg")},
                new Subject { Name = "English", SchoolYear = "2020-2021", YearGrade = 12, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "inglishMista@gmail.bg")},
                new Subject { Name = "Maths", SchoolYear = "2020-2021", YearGrade = 12, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "iIvanova@gmail.bg")},
                new Subject { Name = "Bulgarian", SchoolYear = "2020-2021", YearGrade = 12, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "petko.p@abv.bg")},
            };

            var subjects8thGradeNTBG = new List<Subject>
            {
                new Subject { Name = "Finance", SchoolYear = "2020-2021", YearGrade = 8, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "bankata@abv.bg")},
                new Subject { Name = "Arts", SchoolYear = "2020-2021", YearGrade = 8, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "yaya@abv.bg")},
                new Subject { Name = "Maths", SchoolYear = "2020-2021", YearGrade = 8, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "ivan-na-kvadrat@mail.bg")},
                new Subject { Name = "Bulgarian", SchoolYear = "2020-2021", YearGrade = 8, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "rboji@abv.bg")},
            };

            var subjects12thGradeNTBG = new List<Subject>
            {
                new Subject { Name = "Finance", SchoolYear = "2020-2021", YearGrade = 12, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "bankata@abv.bg")},
                new Subject { Name = "Arts", SchoolYear = "2020-2021", YearGrade = 12, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "yaya@abv.bg")},
                new Subject { Name = "Maths", SchoolYear = "2020-2021", YearGrade = 12, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "ivan-na-kvadrat@mail.bg")},
                new Subject { Name = "Bulgarian", SchoolYear = "2020-2021", YearGrade = 12, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "rboji@abv.bg")},
            };

            var primarySchool = await dbContext.Schools.Include(s => s.Classes).FirstOrDefaultAsync(s => s.Type == SchoolType.PrimarySchool);
            if (primarySchool != null)
            {
                var class5A = primarySchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 5);
                foreach (var subject in subjects5thGradePrimarySchool)
                {
                    foreach (var student in class5A.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    await dbContext.AddAsync(subject);
                }
                var class7A = primarySchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 7);
                var class7B = primarySchool.Classes.FirstOrDefault(c => c.Letter == 'B' && c.Year == 7);
                var class7C = primarySchool.Classes.FirstOrDefault(c => c.Letter == 'C' && c.Year == 7);
                foreach (var subject in subjects7thGradePrimarySchool)
                {
                    foreach (var student in class7A.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    foreach (var student in class7B.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    foreach (var student in class7C.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    await dbContext.AddAsync(subject);
                }

            }
            var secondEnglishLanguageSchool = await dbContext.Schools.Include(s => s.Classes).FirstOrDefaultAsync(s => s.Type == SchoolType.HighSchool && s.Name.Contains("Second English Language High School"));
            if (secondEnglishLanguageSchool != null)
            {
                var class8A = secondEnglishLanguageSchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 8);
                foreach (var subject in subjects8thGrade2ELS)
                {
                    foreach (var student in class8A.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    await dbContext.AddAsync(subject);
                }
                var class12A = secondEnglishLanguageSchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 12);
                var class12B = secondEnglishLanguageSchool.Classes.FirstOrDefault(c => c.Letter == 'B' && c.Year == 12);
                var class12C = secondEnglishLanguageSchool.Classes.FirstOrDefault(c => c.Letter == 'C' && c.Year == 12);
                foreach (var subject in subjects12thGrade2ELS)
                {
                    foreach (var student in class12A.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    foreach (var student in class12B.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    foreach (var student in class12C.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    await dbContext.AddAsync(subject);
                }
            }
            var nationalTradingAndBankingSchool = await dbContext.Schools.Include(s => s.Classes).FirstOrDefaultAsync(s => s.Type == SchoolType.HighSchool && s.Name.Contains("National Trade and Banking High School"));
            if (nationalTradingAndBankingSchool != null)
            {
                var class8A = nationalTradingAndBankingSchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 8);
                var class8C = nationalTradingAndBankingSchool.Classes.FirstOrDefault(c => c.Letter == 'C' && c.Year == 8);
                foreach (var subject in subjects8thGradeNTBG)
                {
                    foreach (var student in class8A.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    foreach (var student in class8C.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    await dbContext.AddAsync(subject);
                }
                var class12A = nationalTradingAndBankingSchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 12);
                var class12B = nationalTradingAndBankingSchool.Classes.FirstOrDefault(c => c.Letter == 'B' && c.Year == 12);
                foreach (var subject in subjects12thGradeNTBG)
                {
                    foreach (var student in class12A.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    foreach (var student in class12B.Students)
                    {
                        subject.StudentSubjects.Add(new StudentSubject
                        {
                            Student = student,
                            Subject = subject
                        });
                    }
                    await dbContext.AddAsync(subject);
                }
            }
        }

        private async Task SeedClasses(ApplicationDbContext dbContext)
        {
            if (dbContext.Classes.Any())
            {
                return;
            }

            var primarySchoolClasses = new List<Class>
            {
                //Classes for 142OU 
                new Class{Letter = 'A', Year = 5, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "geography@abv.bg")},
                new Class{Letter = 'A', Year = 7, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "loshataDobri@abv.bg")},
                new Class{Letter = 'B', Year = 7, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "a.math@abv.bg")},
                new Class{Letter = 'C', Year = 7, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "bgD@abv.bg")},
            };

            var secondEnglishLanguageSchoolClasses = new List<Class>
            {
                //Classes for 2ELS                
                new Class{Letter = 'A', Year = 8, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "y.apostolov@abv.bg")},
                new Class{Letter = 'A', Year = 12, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "petko.p@abv.bg")},
                new Class{Letter = 'B', Year = 12, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "iIvanova@gmail.bg")},
                new Class{Letter = 'C', Year = 12, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "inglishMista@gmail.bg")},
            };

            var nationalTradeAndBankingSchoolClasses = new List<Class>
            {
                //Classes for NTBG                
                new Class{Letter = 'A', Year = 8, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "ivan-na-kvadrat@mail.bg")},
                new Class{Letter = 'C', Year = 8, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "rboji@abv.bg")},
                new Class{Letter = 'A', Year = 12, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "yaya@abv.bg")},
                new Class{Letter = 'B', Year = 12, YearCreated = DateTime.UtcNow.Year, Teacher = await dbContext.Teachers.FirstOrDefaultAsync(t => t.Email == "bankata@abv.bg")},
            };

            var allClasses = new List<Class>(14);
            allClasses.AddRange(secondEnglishLanguageSchoolClasses);
            allClasses.AddRange(nationalTradeAndBankingSchoolClasses);
            allClasses.AddRange(primarySchoolClasses);

            //NOTE: University won't have classes, it has directly subjects and students

            var counter = 0;
            foreach (var schoolClass in allClasses)
            {
                await dbContext.AddAsync(schoolClass);
            }

            var primarySchool = await dbContext.Schools.FirstOrDefaultAsync(s => s.Type == SchoolType.PrimarySchool);
            if (primarySchool != null)
            {
                primarySchool.Classes = primarySchoolClasses;
                dbContext.Update(primarySchool);
            }

            var secondEnglishLanguageSchool = await dbContext.Schools.FirstOrDefaultAsync(s => s.Type == SchoolType.HighSchool && s.Name.Contains("Second English Language High School"));
            if (secondEnglishLanguageSchool != null)
            {
                secondEnglishLanguageSchool.Classes = secondEnglishLanguageSchoolClasses;
                dbContext.Update(secondEnglishLanguageSchool);
            }

            var ntbg = await dbContext.Schools.FirstOrDefaultAsync(s => s.Type == SchoolType.HighSchool && s.Name.Contains("National Trade and Banking High School"));
            if (ntbg != null)
            {
                ntbg.Classes = nationalTradeAndBankingSchoolClasses;
                dbContext.Update(ntbg);
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task SeedStudents(ApplicationDbContext dbContext)
        {
            if (dbContext.Students.Any())
            {
                return;
            }

            var primarySchoolStudents5A = new List<Student>()
            {
                new Student{FirstName="Dafincho", LastName="Dafidov", BirthDate=new DateTime(2010, 12, 15), PersonalIdentificationNumber="1012150000", Username="Dafkata010", Email="dafkata10@abv.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Slavka", LastName="Slavkova", BirthDate=new DateTime(2010, 1, 10), PersonalIdentificationNumber="1001101111", Username="SlaFcheto", Email="slafcheto@abv.bg", UniqueId = _idGeneratoService.GenerateStudentId()}
            };

            var primarySchoolStudents7A = new List<Student>()
            {
                new Student{FirstName="Rumen", LastName="Rumenov", BirthDate=new DateTime(2008, 12, 15), PersonalIdentificationNumber="0812150000", Username="RumRum", Email="r.rumenov@gmail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Boyko", LastName="Boykov", BirthDate=new DateTime(2008, 1, 10), PersonalIdentificationNumber="0801101111", Username="BB_Vojda", Email="bb_v@abv.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Cvetko", LastName="Cvetkov", BirthDate=new DateTime(2008, 10, 29), PersonalIdentificationNumber="0810291111", Username="CC", Email="cc@mail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
            };

            var primarySchoolStudents7B = new List<Student>()
            {
                new Student{FirstName="Albena", LastName="Litstein", BirthDate=new DateTime(2008, 11, 11), PersonalIdentificationNumber="0811110000", Username="Bencheto", Email="al_beny@abv.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Rosko", LastName="Roskov", BirthDate=new DateTime(2008, 5, 15), PersonalIdentificationNumber="0805151111", Username="RosKO", Email="ros_ros@abv.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Alberto", LastName="Svqtkov", BirthDate=new DateTime(2008, 10, 18), PersonalIdentificationNumber="0810101111", Username="Albeerto", Email="berto@gmail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
            };

            var primarySchoolStudents7C = new List<Student>()
            {
                new Student{FirstName="Rosalinda", LastName="Balareva", BirthDate=new DateTime(2008, 11, 11), PersonalIdentificationNumber="0811110000", Username="RosyBal", Email="rosybalareva@gmail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Andrew", LastName="Feshuna", BirthDate=new DateTime(2008, 8, 15), PersonalIdentificationNumber="0808151111", Username="AndrewF", Email="andrewfash@abv.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Alex", LastName="Svqtkov", BirthDate=new DateTime(2008, 10, 1), PersonalIdentificationNumber="0810011111", Username="AlexSV", Email="alexSV@mail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
            };

            var elsStudents8A = new List<Student>()
            {
                new Student{FirstName="Rumen", LastName="Radoychev", BirthDate=new DateTime(2007, 12, 15), PersonalIdentificationNumber="0712150000", Username="RumDoi", Email="r.radoychev@gmail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Big", LastName="Boykov", BirthDate=new DateTime(2007, 1, 10), PersonalIdentificationNumber="0701101111", Username="BB_Vojda", Email="bb_v@abv.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Cece", LastName="Cvetkov", BirthDate=new DateTime(2007, 10, 29), PersonalIdentificationNumber="0710291111", Username="CC", Email="cc@mail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
            };

            var elsStudents12A = new List<Student>()
            {
                //ToDo
            };

            var elsStudents12B = new List<Student>()
            {
                //ToDo
            };

            var elsStudents12C = new List<Student>()
            {
                //ToDo
            };

            var ntbgStudents8A = new List<Student>()
            {
                new Student{FirstName="Samoten", LastName="Voin", BirthDate=new DateTime(2007, 12, 12), PersonalIdentificationNumber="0712120000", Username="SamVoin", Email="SamVoin@abv.bg", UniqueId = _idGeneratoService.GenerateStudentId()}
            };

            var ntbgStudents8C = new List<Student>()
            {
                new Student{FirstName="Bala", LastName="Rosalindova", BirthDate=new DateTime(2007, 11, 11), PersonalIdentificationNumber="0711110000", Username="BalRosy", Email="balrosalindova@gmail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Freddy", LastName="Feshuna", BirthDate=new DateTime(2007, 8, 15), PersonalIdentificationNumber="0708151111", Username="FreddyF", Email="Freddyfash@abv.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Alex", LastName="Alexandrov", BirthDate=new DateTime(2007, 10, 1), PersonalIdentificationNumber="0710011111", Username="AlexAlex", Email="alexx@mail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
            };

            var ntbgStudents12A = new List<Student>()
            {
                //ToDo
            };

            var ntbgStudents12B = new List<Student>()
            {
                //ToDo
            };

            var nbuStudents = new List<Student>()
            {
                new Student{FirstName="Noviqt", LastName="Bulgarin", BirthDate=new DateTime(1991, 11, 11), PersonalIdentificationNumber="9111110000", Username="NewBulg", Email="new_bulg@gmail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Radomitko", LastName="Mitkoradov", BirthDate=new DateTime(1995, 12, 11), PersonalIdentificationNumber="9521110000", Username="AdMir", Email="AdMir@abv.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Peter", LastName="Peterov", BirthDate=new DateTime(1999, 5, 5), PersonalIdentificationNumber="9905050000", Username="Pepe", Email="petpet@mail.bg", UniqueId = _idGeneratoService.GenerateStudentId()},
                new Student{FirstName="Edmund", LastName="Shwartz", BirthDate=new DateTime(1988, 1, 31), PersonalIdentificationNumber="8801310000", Username="edysw", Email="edysw@gmail.bg", UniqueId = _idGeneratoService.GenerateStudentId()}
            };

            if (nbuStudents.Any(s => s.UniqueId == null))
            {
                var exampleStudent = nbuStudents.FirstOrDefault(s => s.UniqueId == null);
                Console.WriteLine($"NBU student with null id: {exampleStudent.Email}");
            }

            var primarySchoolStudents = new List<Student>();
            primarySchoolStudents.AddRange(primarySchoolStudents5A);
            primarySchoolStudents.AddRange(primarySchoolStudents7A);
            primarySchoolStudents.AddRange(primarySchoolStudents7B);
            primarySchoolStudents.AddRange(primarySchoolStudents7C);

            if (primarySchoolStudents.Any(s => s.UniqueId == null))
            {
                var exampleStudent = primarySchoolStudents.FirstOrDefault(s => s.UniqueId == null);
                Console.WriteLine($"Primary school student with null id: {exampleStudent.Email}");
            }

            // 142 OU students relations
            var primarySchool = await dbContext.Schools.Include(s => s.Classes).FirstOrDefaultAsync(s => s.Type == SchoolType.PrimarySchool);
            if (primarySchool != null)
            {
                primarySchool.Students = primarySchoolStudents;
                dbContext.Update(primarySchool);

                var class5A = primarySchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 5);
                class5A.Students = primarySchoolStudents5A;
                dbContext.Update(class5A);

                var class7A = primarySchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 7);
                class7A.Students = primarySchoolStudents7A;
                dbContext.Update(class7A);

                var class7B = primarySchool.Classes.FirstOrDefault(c => c.Letter == 'B' && c.Year == 7);
                class7B.Students = primarySchoolStudents7B;
                dbContext.Update(class7B);

                var class7C = primarySchool.Classes.FirstOrDefault(c => c.Letter == 'C' && c.Year == 7);
                class7C.Students = primarySchoolStudents7C;
                dbContext.Update(class7C);
            }

            var secondEnglishLanguageSchoolStudents = new List<Student>();
            secondEnglishLanguageSchoolStudents.AddRange(elsStudents8A);
            secondEnglishLanguageSchoolStudents.AddRange(elsStudents12A);
            secondEnglishLanguageSchoolStudents.AddRange(elsStudents12B);
            secondEnglishLanguageSchoolStudents.AddRange(elsStudents12C);

            if (secondEnglishLanguageSchoolStudents.Any(s => s.UniqueId == null))
            {
                var exampleStudent = secondEnglishLanguageSchoolStudents.FirstOrDefault(s => s.UniqueId == null);
                Console.WriteLine($"2ELS student with null id: {exampleStudent.Email}");
            }

            // 2 els students relations
            var secondEnglishLanguageSchool = await dbContext.Schools.Include(s => s.Classes).FirstOrDefaultAsync(s => s.Type == SchoolType.HighSchool && s.Name.Contains("Second English Language High School"));
            if (secondEnglishLanguageSchool != null)
            {
                secondEnglishLanguageSchool.Students = secondEnglishLanguageSchoolStudents;
                dbContext.Update(secondEnglishLanguageSchool);

                var elsClass8A = secondEnglishLanguageSchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 8);
                elsClass8A.Students = elsStudents8A;
                dbContext.Update(elsClass8A);

                var elsClass12A = secondEnglishLanguageSchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 12);
                elsClass12A.Students = elsStudents12A;
                dbContext.Update(elsClass12A);

                var elsClass12B = secondEnglishLanguageSchool.Classes.FirstOrDefault(c => c.Letter == 'B' && c.Year == 12);
                elsClass12B.Students = elsStudents12B;
                dbContext.Update(elsClass12B);

                var elsClass12C = secondEnglishLanguageSchool.Classes.FirstOrDefault(c => c.Letter == 'C' && c.Year == 12);
                elsClass12C.Students = elsStudents12C;
                dbContext.Update(elsClass12C);
            }


            var ntbgStudents = new List<Student>();
            ntbgStudents.AddRange(ntbgStudents8A);
            ntbgStudents.AddRange(ntbgStudents8C);
            ntbgStudents.AddRange(ntbgStudents12A);
            ntbgStudents.AddRange(ntbgStudents12B);

            if (ntbgStudents.Any(s => s.UniqueId == null))
            {
                var exampleStudent = ntbgStudents.FirstOrDefault(s => s.UniqueId == null);
                Console.WriteLine($"NTBG student with null id: {exampleStudent.Email}");
            }

            // 2 els students relations
            var nationalTradingAndBankingSchool = await dbContext.Schools.Include(s => s.Classes).FirstOrDefaultAsync(s => s.Type == SchoolType.HighSchool && s.Name.Contains("National Trade and Banking High School"));
            if (nationalTradingAndBankingSchool != null)
            {
                nationalTradingAndBankingSchool.Students = ntbgStudents;
                dbContext.Update(nationalTradingAndBankingSchool);

                var ntbgClass8A = nationalTradingAndBankingSchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 8);
                ntbgClass8A.Students = ntbgStudents8A;
                dbContext.Update(ntbgClass8A);

                var ntbgClass8C = nationalTradingAndBankingSchool.Classes.FirstOrDefault(c => c.Letter == 'C' && c.Year == 8);
                ntbgClass8C.Students = ntbgStudents8C;
                dbContext.Update(ntbgClass8C);

                var ntbgClass12A = nationalTradingAndBankingSchool.Classes.FirstOrDefault(c => c.Letter == 'A' && c.Year == 12);
                ntbgClass12A.Students = ntbgStudents12A;
                dbContext.Update(ntbgClass12A);

                var ntbgClass12B = nationalTradingAndBankingSchool.Classes.FirstOrDefault(c => c.Letter == 'B' && c.Year == 12);
                ntbgClass12B.Students = ntbgStudents12B;
                dbContext.Update(ntbgClass12B);
            }

            var nbu = await dbContext.Schools.FirstOrDefaultAsync(s => s.Type == SchoolType.University && s.Name.Contains("New Bulgarian University"));
            if (nbu != null)
            {
                nbu.Students = nbuStudents;
                dbContext.Update(nbu);
            }


            await dbContext.SaveChangesAsync();
        }
    }
}