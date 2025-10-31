using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CRUDApi.Models
{
    public class DepartmentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName ="Varchar")]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        [Column(TypeName ="Varchar")]
        public string Description { get; set; }
        public bool? Status {  get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }
    }
}
