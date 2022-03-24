using System;
using System.ComponentModel.DataAnnotations;

namespace ALMS.Model
{
    public class OrganizationDto : IDtoEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUserName { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}