using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class DetailsOrder
{
    public int IdDetailsOrder { get; set; }

    public int Count { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal SubTotal { get; set; }

    public int IdOrder { get; set; }

    public int? IdGarmentService { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? SelectedColor { get; set; }

    public string? Details { get; set; }

    public string? SelectedSize { get; set; }

    public int? IdService { get; set; }

    public string? ServiceName { get; set; }

    public string? ImageUrl { get; set; }

    public virtual GarmentService? IdGarmentServiceNavigation { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Service? IdServiceNavigation { get; set; }
}
