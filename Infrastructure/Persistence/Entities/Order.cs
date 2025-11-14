namespace Infrastructure.Persistence.Entities;

public partial class Order
{
    public int IdOrder { get; set; }

    public DateOnly CreationDate { get; set; }

    public decimal Total { get; set; }

    public int IdUser { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? IdCustom { get; set; }

    public virtual ICollection<DetailsOrder> DetailsOrders { get; set; } = new List<DetailsOrder>();

    public virtual Custom? IdCustomNavigation { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
