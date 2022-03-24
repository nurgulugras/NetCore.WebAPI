
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALMS.Model
{
    public class EntityBase : IEntityBase
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [ForeignKey("CreateUser")]
        [Required]
        public int CreateUserId { get; set; }
        public virtual User CreateUser { get; set; }

    }
}