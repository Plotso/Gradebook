namespace Gradebook.Web.ViewModels.Subject
{
    using Data.Models.Grades;
    using Services.Mapping;

    public class GradeViewModel : IMapFrom<Grade>
    {
        public int Id { get; set; }

        public decimal Value { get; set; } //ex: 6.00, 4.50, 5.75

        public GradePeriod Period { get; set; }

        public GradeType Type { get; set; }
    }
}