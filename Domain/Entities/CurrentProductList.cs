using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Keyless]
public partial class CurrentProductList
{
    [Column("ProductID")]
    public int ProductId { get; set; }

    [StringLength(40)]
    public string ProductName { get; set; } = null!;
}
