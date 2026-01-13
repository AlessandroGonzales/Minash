using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class Custom
{
    public int IdCustom { get; set; }

    public string CustomerDetails { get; set; } = null!;

    public List<string> ImageUrl { get; set; } = null!;

    public int Count { get; set; }

    public DateOnly CreationDate { get; set; }

    public int IdUser { get; set; }

    public int? IdService { get; set; }

    public int? IdGarment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? IdGarmentService { get; set; }

    public string? SelectedColor { get; set; }

    public string? SelectedSize { get; set; }

    public int? IdOrder { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? CustomTotal { get; set; }

    public string? CustomName { get; set; }

    public virtual Garment? IdGarmentNavigation { get; set; }

    public virtual GarmentService? IdGarmentServiceNavigation { get; set; }

    public virtual Order? IdOrderNavigation { get; set; }

    public virtual Service? IdServiceNavigation { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
