using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public partial class MinashDbContext : DbContext
{
    public MinashDbContext()
    {
    }

    public MinashDbContext(DbContextOptions<MinashDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountingRecord> AccountingRecords { get; set; }

    public virtual DbSet<Custom> Customs { get; set; }

    public virtual DbSet<DetailsOrder> DetailsOrders { get; set; }

    public virtual DbSet<Garment> Garments { get; set; }

    public virtual DbSet<GarmentService> GarmentServices { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=minash;Username=postgres;Password=Jamancapiero85.");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("custom_state", new[] { "Pendiente", "En proceso", "Terminado" })
            .HasPostgresEnum("garment_state", new[] { "Activo", "Inactivo" })
            .HasPostgresEnum("order_state", new[] { "Pendiente", "En proceso", "Enviado", "Completado" })
            .HasPostgresEnum("payment_method", new[] { "Efectivo", "Transferencia" })
            .HasPostgresEnum("payment_state", new[] { "Pendiente", "Aceptado", "Rechazado" })
            .HasPostgresEnum("service_state", new[] { "Activo", "Inactivo" })
            .HasPostgresEnum("user_state", new[] { "Activo", "Inactivo" });

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentMethod)
            .HasColumnName("payment_method")
            .HasConversion<string>();


        modelBuilder.Entity<AccountingRecord>(entity =>
        {
            entity.HasKey(e => e.IdAccountingRecord).HasName("accounting_records_pkey");

            entity.ToTable("accounting_records");

            entity.Property(e => e.IdAccountingRecord).HasColumnName("id_accounting_record");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Details).HasColumnName("details");
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("entry_date");
            entity.Property(e => e.IdPay).HasColumnName("id_pay");
            entity.Property(e => e.Total)
                .HasPrecision(12, 2)
                .HasColumnName("total");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdPayNavigation).WithMany(p => p.AccountingRecords)
                .HasForeignKey(d => d.IdPay)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("accounting_records_id_pay_fkey");
        });

        modelBuilder.Entity<Custom>(entity =>
        {
            entity.HasKey(e => e.IdCustom).HasName("custom_pkey");

            entity.ToTable("custom");

            entity.Property(e => e.IdCustom).HasColumnName("id_custom");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("creation_date");
            entity.Property(e => e.CustomerDetails).HasColumnName("customer_details");
            entity.Property(e => e.IdGarment).HasColumnName("id_garment");
            entity.Property(e => e.IdService).HasColumnName("id_service");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("image_url");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdGarmentNavigation).WithMany(p => p.Customs)
                .HasForeignKey(d => d.IdGarment)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("custom_id_garment_fkey");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Customs)
                .HasForeignKey(d => d.IdService)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("custom_id_service_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Customs)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("custom_id_user_fkey");
        });

        modelBuilder.Entity<DetailsOrder>(entity =>
        {
            entity.HasKey(e => e.IdDetailsOrder).HasName("details_order_pkey");

            entity.ToTable("details_order");

            entity.Property(e => e.IdDetailsOrder).HasColumnName("id_details_order");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IdGarmentService).HasColumnName("id_garment_service");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.SubTotal)
                .HasPrecision(12, 2)
                .HasColumnName("sub_total");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(12, 2)
                .HasColumnName("unit_price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdGarmentServiceNavigation).WithMany(p => p.DetailsOrders)
                .HasForeignKey(d => d.IdGarmentService)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("details_order_id_garment_service_fkey");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.DetailsOrders)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("details_order_id_order_fkey");
        });

        modelBuilder.Entity<Garment>(entity =>
        {
            entity.HasKey(e => e.IdGarment).HasName("garments_pkey");

            entity.ToTable("garments");

            entity.HasIndex(e => e.GarmentName, "garments_garment_name_key").IsUnique();

            entity.HasIndex(e => e.GarmentName, "idx_garments_name");

            entity.Property(e => e.IdGarment).HasColumnName("id_garment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.GarmentDetails).HasColumnName("garment_details");
            entity.Property(e => e.GarmentName)
                .HasMaxLength(50)
                .HasColumnName("garment_name");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("image_url");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<GarmentService>(entity =>
        {
            entity.HasKey(e => e.IdGarmentService).HasName("garment_service_pkey");

            entity.ToTable("garment_service");

            entity.HasIndex(e => new { e.IdService, e.IdGarment }, "unique_garment_service").IsUnique();

            entity.Property(e => e.IdGarmentService).HasColumnName("id_garment_service");
            entity.Property(e => e.AdditionalPrice)
                .HasPrecision(12, 2)
                .HasColumnName("additional_price");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.IdGarment).HasColumnName("id_garment");
            entity.Property(e => e.IdService).HasColumnName("id_service");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("image_url");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdGarmentNavigation).WithMany(p => p.GarmentServices)
                .HasForeignKey(d => d.IdGarment)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("garment_service_id_garment_fkey");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.GarmentServices)
                .HasForeignKey(d => d.IdService)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("garment_service_id_service_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("creation_date");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Total)
                .HasPrecision(12, 2)
                .HasColumnName("total");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("orders_id_user_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IdPay).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.HasIndex(e => new { e.IdOrder, e.ExternalPaymentId }, "ix_payments_id_order_external");

            entity.HasIndex(e => e.ExternalPaymentId, "ux_payments_external_payment_id").IsUnique();

            entity.Property(e => e.IdPay).HasColumnName("id_pay");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValueSql("'ARS'::character varying")
                .HasColumnName("currency");
            entity.Property(e => e.ExternalPaymentId)
                .HasMaxLength(100)
                .HasColumnName("external_payment_id");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdempotencyKey)
                .HasMaxLength(100)
                .HasColumnName("idempotency_key");
            entity.Property(e => e.Installments).HasColumnName("installments");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("payment_date");
            entity.Property(e => e.Provider)
                .HasMaxLength(50)
                .HasDefaultValueSql("'MercadoPago'::character varying")
                .HasColumnName("provider");
            entity.Property(e => e.ProviderResponse)
                .HasColumnType("jsonb")
                .HasColumnName("provider_response");
            entity.Property(e => e.ReceiptImageUrl)
                .HasMaxLength(255)
                .HasColumnName("receipt_image_url");
            entity.Property(e => e.ReceiptUrl)
                .HasMaxLength(255)
                .HasColumnName("receipt_url");
            entity.Property(e => e.Total)
                .HasPrecision(12, 2)
                .HasColumnName("total");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(100)
                .HasColumnName("transaction_code");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.Verified)
                .HasDefaultValue(false)
                .HasColumnName("verified");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("payments_id_order_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.RoleDetails).HasColumnName("role_details");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .HasColumnName("role_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("services_pkey");

            entity.ToTable("services");

            entity.HasIndex(e => e.ServiceName, "idx_services_name");

            entity.HasIndex(e => e.ServiceName, "services_service_name_key").IsUnique();

            entity.Property(e => e.IdService).HasColumnName("id_service");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("image_url");
            entity.Property(e => e.Price)
                .HasPrecision(12, 2)
                .HasColumnName("price");
            entity.Property(e => e.ServiceDetails).HasColumnName("service_details");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(50)
                .HasColumnName("service_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "idx_users_email");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Phone, "users_phone_key").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullAddress)
                .HasMaxLength(255)
                .HasColumnName("full_address");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Province)
                .HasMaxLength(100)
                .HasColumnName("province");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("registration_date");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_id_role_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
