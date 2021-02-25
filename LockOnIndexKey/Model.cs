using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LockOnIndexKey
{
    [Table("DeadlockIndexTest")]
    public class Model
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(10)]
        public string AdditionalInfo { get; set; }
    }
}
