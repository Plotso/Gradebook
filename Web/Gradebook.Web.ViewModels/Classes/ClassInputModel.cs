namespace Gradebook.Web.ViewModels.Classes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Services.Mapping;

    public class ClassInputModel : IMapFrom<Class>, IMapTo<Class>
    {
        [Required]
        public char Letter { get; set; }

        [Range(1, 12)]
        public int Year { get; set; } // 1st year, 2nd year, 3rd year student and etc.

        [Range(1900, int.MaxValue,
            ErrorMessage = "The year that the class has been created should be greater than 1900")]
        public int YearCreated { get; set; } = DateTime.UtcNow.Year;

        [Required]
        public string TeacherId { get; set; }
    }
}