using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class Video
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? Type { get; set; }
}
