using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace productservice.Model
{
    public class Product
    {
        public int productId { get; set; }
        public int? parentProducId { get; set; }
        [Required]
        [MaxLength(50)]
        public string producName { get; set; }
        [MaxLength(100)]
        public string producDescription { get; set; }
        [MaxLength(50)]
        public string producCode { get; set; }
        [MaxLength(50)]
        public string productGTIN { get; set; }
        [Column("childrenProductsIds", TypeName = "integer[]")]
        public int[] childrenProductsIds { get; set; }
        [DefaultValue(true)]
        public bool enabled { get; set; }
    }
}