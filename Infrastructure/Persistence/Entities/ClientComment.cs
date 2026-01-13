using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class ClientComment
{
    public int IdComment { get; set; }

    public string Feedback { get; set; } = null!;

    public int IdUser { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;
}
