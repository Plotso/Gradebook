namespace Gradebook.Data.Models
{
    using Common;
    using Common.Models;

    public class Principal : BasePersonModel
    {
        public Principal()
        {
            UserType = UserType.Principal;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual School School { get; set; }
    }
}