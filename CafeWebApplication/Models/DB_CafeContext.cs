using Microsoft.EntityFrameworkCore;

namespace CafeWebApplication
{
    public partial class DB_CafeContext : DbContext
    {
        public DB_CafeContext()
        {
        }

        public DB_CafeContext(DbContextOptions<DB_CafeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cafe> Cafes { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<ItemType> ItemTypes { get; set; } = null!;
        public virtual DbSet<MenuItem> MenuItems { get; set; } = null!;
        public virtual DbSet<MenuOrder> MenuOrders { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Table> Tables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= DESKTOP-H9T34QQ\\SQLEXPRESS01; Database=DB_Cafe; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cafe>(entity =>
            {
                entity.Property(e => e.Address)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.DateOfEmployment).HasColumnType("date");

                entity.Property(e => e.DateOfRelease).HasColumnType("date");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Cafe)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.CafeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Employees_Cafes");
            });

            modelBuilder.Entity<ItemType>(entity =>
            {
                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("smallmoney");

                entity.HasOne(d => d.Cafe)
                    .WithMany(p => p.MenuItems)
                    .HasForeignKey(d => d.CafeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_MenuItems_Cafes");

                entity.HasOne(d => d.ItemType)
                    .WithMany(p => p.MenuItems)
                    .HasForeignKey(d => d.ItemTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_MenuItems_ItemTypes");
            });

            modelBuilder.Entity<MenuOrder>(entity =>
            {
                entity.HasOne(d => d.MenuItem)
                    .WithMany(p => p.MenuOrders)
                    .HasForeignKey(d => d.MenuItemId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_MenuOrders_MenuItems");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.MenuOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_MenuOrders_Orders");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.TableId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Orders_Tables");

                entity.HasOne(d => d.Waiter)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.WaiterId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Orders_Employees");
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.HasOne(d => d.Cafe)
                    .WithMany(p => p.Tables)
                    .HasForeignKey(d => d.CafeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Tables_Cafes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
