namespace Gradebook.Services.Data
{
    using System;
    using System.Threading;

    public class IdGeneratorService : IIdGeneratorService
    {
        private static int _idsCounter;

        public IdGeneratorService()
        {
            _idsCounter = 0;
        }

        public string GeneratePrincipalId() => GenerateId('G');

        public string GenerateTeacherId() => GenerateId('T');

        public string GenerateParentId() => GenerateId('P');

        public string GenerateStudentId() => GenerateId('S');


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