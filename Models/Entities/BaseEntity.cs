using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Librarian.Models.Entities
{
    public class BaseEntity
    {
   //     [Column(TypeName = "datetime2")]
        public DateTime CreatedDate { get; set; }

    //    [Column(TypeName = "datetime2")]
        public DateTime? UpdatedDate { get; set; }

    //    [Column(TypeName = "datetime2")]
        public DateTime? DeletedDate { get; set; }

        [Column(TypeName = "boolean")]
        public bool IsDeleted { get; set; } = false;
    }
}
