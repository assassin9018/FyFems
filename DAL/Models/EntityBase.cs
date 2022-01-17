using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class EntityBase
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
