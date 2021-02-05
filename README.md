# Gradebook
Project for CITB579 course in New Bulgarian University. The website is a basic web gradebook that can has many roles and functionalities.

It consists of many pages, most notable of which are:
* **Home** - current user can see all schools to which he has access
* **SubjectsList** - current user can see all subjects that he is authorized to see
* **Subject details** - page where user can see all grades and absences for each student. It can be filter by term (First/Second) and also teachers have functionalities to add new grade or absence
* **ClassesList** - current user can see all classes that he is authorized to see
* **Class details** - page where user can see information about each student in a class together with information about it's parents.
* **SchoolsList** - admin specific page used for management of schools

There are also pages for privacy policy, user profile page, login, registration, multiple role specific and sever others.

The platform has the following roles:
* **Admin** - has all the access as other roles. Also has functionality to add schools and principals.
* **Principal** - has all the access as teachers, students and parents. Also has the functionality to create new teacher, student, parent, subject and to assign different students to classes, classes to subjects and so on.
* **Teacher** - has all the access as students and parents. Also has the functionality to create, edit and delete grades and absences for courses that the teacher is assigned to.
* **Student** - can view his classes, subjects, grades, absences and so on. Has only ReadOnly access.
* **Parent** - can view classes, subjects, grades, absences and so on for each of it's children. Has only ReadOnly access.


## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

In order to be sure you can run the project, make sure you have the following frameworks installed on your PC:
* **NET Standard 2.0 compatible frameworks** - **NET Core 3.1** & **NET Framework 4.6.1** (should be installed with VisualStudio/Rider, double check it just in case)
* **SQL Server 2019** - the project is built and tested on EF 3.1.10 which, is using latest SQL Server version

It's good to have the following software products installed in order to be sure the project is running as expected:
* **VisualStudio 2019 / Rider 2020** - built and tested on both of those IDEs, the project should also be running on any newer version as long as it supports the above mentioned frameworks
* **SQL Server Management Studio (SSMS) / Azure Data Studio** - This one is not required at all, but if you want to check what happens in the database, it's good to have it. It's recommended to use latest version, the project has been tested on SSMS 2018 & Azure Data Studio 2020 and it's working fine.

### Installation

If you want to have **custom database name**, go to [appsettings.json](Web/Gradebook.Web/appsettings.json) file and change **_THAT__** to whatever name you would like:
```
"ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=THAT_;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
```

Before initially running the project, go to [Gradebook.Data](Data/Gradebook.Data) folder (the one containing Gradebook.Data.csproj file) and execute the following commands:

```
dotnet ef migrations add initialCreate
```
```
dotnet ef database update 
```
Those commands are required in order for the project to correctly build the database before running it.


## Built With

* [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-3.1) - The web framework
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/) - The ORM framework
* [Bootstrap](https://getbootstrap.com) - Front-end kit used for better styling
* [jQuery](https://api.jquery.com/jquery.ajax/) - jQuery is used in sfor adding transitions, animations and so on


## Contribution

* **Ilian Ganchosov** - [Plotso](https://github.com/Plotso)
* **Kristiyan Knizharov** - [KristiyanKnizharov](https://github.com/KristiyanKnizharov)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Sneak Peak at home page
![HomePagePresentation](https://github.com/Plotso/Gradebook/blob/main/HomePagePresentation.PNG?raw=true)