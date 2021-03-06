﻿namespace Gradebook.Web.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Services.Mapping;

    public class ParentViewModel : IMapFrom<Parent>
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        
        public string UniqueId { get; set; }
        
        public ICollection<StudentParentViewModel> StudentParents { get; set; }
    }
}