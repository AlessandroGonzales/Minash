using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class Service
{
    public int IdService { get; set; }

    public string ServiceName { get; set; } = null!;

    public string ServiceDetails { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? ImageUrl { get; set; }

    public List<string>? Colors { get; set; }

    public virtual ICollection<Custom> Customs { get; set; } = new List<Custom>();

    public virtual ICollection<DetailsOrder> DetailsOrders { get; set; } = new List<DetailsOrder>();

    public virtual ICollection<GarmentService> GarmentServices { get; set; } = new List<GarmentService>();
}
