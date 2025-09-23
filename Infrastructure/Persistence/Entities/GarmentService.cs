namespace Infrastructure.Persistence.Entities;

public partial class GarmentService
{
    public int IdGarmentService { get; set; }

    public decimal AdditionalPrice { get; set; }

    public int IdService { get; set; }

    public int IdGarment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<DetailsOrder> DetailsOrders { get; set; } = new List<DetailsOrder>();

    public virtual Garment IdGarmentNavigation { get; set; } = null!;

    public virtual Service IdServiceNavigation { get; set; } = null!;
}
