namespace Gradebook.Web.ViewModels.Principal
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Services.Mapping;

    public class ClassViewModel : IMapFrom<Class>
    {
        public int Id { get; set; }

        [Required]
        public char Letter { get; set; }

        public int Year { get; set; } // 1st year, 2nd year, 3rd year student and etc.

        public int YearCreated { get; set; }  // випуск
    }
}