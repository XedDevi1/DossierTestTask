using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MainTestTask.Models
{
    public class Dossier
    {
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int OrderNumber { get; set; }

        [MaxLength(50)]
        public string? SectionCode { get; set; }

        [MaxLength(500)]
        public string? Name { get; set; }

        // Навигационные свойства
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Dossier Parent { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Dossier> Children { get; set; }
    }
}
