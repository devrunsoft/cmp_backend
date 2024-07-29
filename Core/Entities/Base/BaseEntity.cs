using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoutDirect.Core.Entities.Base
{ 
    public class BaseEntity<T> : IPersistentObject<T>
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key]
        //[Column("id")]
        public T Id { get; set; }
    }
}
