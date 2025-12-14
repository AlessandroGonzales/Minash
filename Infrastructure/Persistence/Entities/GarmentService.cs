using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class GarmentService
{
    public int IdGarmentService { get; set; }

    public decimal AdditionalPrice { get; set; }

    public int IdService { get; set; }

    public int IdGarment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<string>? ImageUrl { get; set; }

    public string? GarmentServiceName { get; set; }

    public string? GarmentServiceDetails { get; set; }

    public List<string>? Colors { get; set; }

    public List<string>? Sizes { get; set; }

    public virtual ICollection<Custom> Customs { get; set; } = new List<Custom>();

    public virtual ICollection<DetailsOrder> DetailsOrders { get; set; } = new List<DetailsOrder>();

    public virtual Garment IdGarmentNavigation { get; set; } = null!;

    public virtual Service IdServiceNavigation { get; set; } = null!;
}
