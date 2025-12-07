using System;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Entities;

public partial class User
{
    public int IdUser { get; set; }

    public string UserName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateOnly RegistrationDate { get; set; }

    public int IdRole { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? ImageUrl { get; set; }

    public string? Province { get; set; }

    public string? City { get; set; }

    public string? FullAddress { get; set; }

    public virtual ICollection<Custom> Customs { get; set; } = new List<Custom>();

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
