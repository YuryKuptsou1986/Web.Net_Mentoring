using System.ComponentModel.DataAnnotations;

namespace Homework.Entities.ViewModel.Product
{
    public abstract class ProductBaseModel
    {
        [Required]
        [StringLength(40, ErrorMessage = "ProductName length can't be more than '40'.")]
        public string ProductName { get; set; }

        public int? SupplierId { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(20, ErrorMessage = "QuantityPerUnit length can't be more than '20'.")]
        public string QuantityPerUnit { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value should be equals or more than '0'")]
        public decimal? UnitPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value should be quals or more than '0'")]
        public short? UnitsInStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value should be quals or more than '0'")]
        public short? UnitsOnOrder { get; set; }

        [Range(0,100)]
        public short? ReorderLevel { get; set; }

        [Required]
        public bool Discontinued { get; set; }
    }
}
