namespace Infrastructure.Persistence.Entities;

public partial class Garment
{
    public int IdGarment { get; set; }

    public string GarmentName { get; set; } = null!;

    public string GarmentDetails { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Custom> Customs { get; set; } = new List<Custom>();

    public virtual ICollection<GarmentService> GarmentServices { get; set; } = new List<GarmentService>();
}
