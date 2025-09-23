namespace Infrastructure.Persistence.Entities;

public partial class Custom
{
    public int IdCustom { get; set; }

    public string CustomerDetails { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int Count { get; set; }

    public DateOnly CreationDate { get; set; }

    public int IdUser { get; set; }

    public int IdService { get; set; }

    public int IdGarment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Garment IdGarmentNavigation { get; set; } = null!;

    public virtual Service IdServiceNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
