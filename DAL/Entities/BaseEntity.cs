using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PK_ID { get; set; }

        public BaseEntity()
        {
            PK_ID = Guid.NewGuid();
        }
    }
}
