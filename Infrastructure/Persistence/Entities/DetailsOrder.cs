namespace Infrastructure.Persistence.Entities;

public partial class DetailsOrder
{
    public int IdDetailsOrder { get; set; }

    public int Count { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal SubTotal { get; set; }

    public int IdOrder { get; set; }

    public int IdGarmentService { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual GarmentService IdGarmentServiceNavigation { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
