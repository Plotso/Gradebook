namespace Gradebook.Services.Data
{
    using System;
    using System.Threading;
    using Common;
    using Interfaces;

    public class IdGeneratorService : IIdGeneratorService
    {
        private static int _idsCounter;

        public IdGeneratorService()
        {
            _idsCounter = 0;
        }

        public string GeneratePrincipalId() => GenerateId(GlobalConstants.PrincipalIdPrefix);

        public string GenerateTeacherId() => GenerateId(GlobalConstants.TeacherIdPrefix);

        public string GenerateParentId() => GenerateId(GlobalConstants.ParentIdPrefix);

        public string GenerateStudentId() => GenerateId(GlobalConstants.StudentIdPrefix);


        private string GenerateId(char firstLetter)
        {
            var year = GetYearIndicator();
            var uniqueIdEnding = Interlocked.Increment(ref _idsCounter);
            return $"{firstLetter}{year}{uniqueIdEnding}";
        }

        private string GetYearIndicator()
        {
            var year = DateTime.UtcNow.Year.ToString();
            return year.Substring(1);
        }
    }
}