using System.ComponentModel.DataAnnotations.Schema;

namespace DbConcurrentUpdateLocks
{
    [Table("Rows", Schema = "dbo")]
    class Model
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Value { get; set; }
    }
}
