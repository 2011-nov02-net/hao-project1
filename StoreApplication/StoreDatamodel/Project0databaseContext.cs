using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace StoreDatamodel
{
    public partial class Project0databaseContext : DbContext
    {
        public Project0databaseContext()
        {
        }

        public Project0databaseContext(DbContextOptions<Project0databaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admincredential> Admincredentials { get; set; }
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<Orderproduct> Orderproducts { get; set; }
        public virtual DbSet<Orderr> Orderrs { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Storecustomer> Storecustomers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admincredential>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__admincre__AB6E616506641EF8");

                entity.ToTable("admincredential");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("password");
            });

            modelBuilder.Entity<Credential>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__credenti__AB6E616524386D51");

                entity.ToTable("credential");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("password");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.Email, "UQ__customer__AB6E6164EF47244D")
                    .IsUnique();

                entity.Property(e => e.Customerid)
                    .HasMaxLength(20)
                    .HasColumnName("customerid");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("lastname");

                entity.Property(e => e.Phonenumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("phonenumber");

                entity.HasOne(d => d.EmailNavigation)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.Email)
                    .HasConstraintName("FK__customer__email__351DDF8C");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.Supplyid)
                    .HasName("PK__inventor__EF33FCB8FE35CA05");

                entity.ToTable("inventory");

                entity.Property(e => e.Supplyid).HasColumnName("supplyid");

                entity.Property(e => e.Productid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("productid");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Storeloc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("storeloc");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK__inventory__produ__46486B8E");

                entity.HasOne(d => d.StorelocNavigation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.Storeloc)
                    .HasConstraintName("FK__inventory__store__45544755");
            });

            modelBuilder.Entity<Orderproduct>(entity =>
            {
                entity.HasKey(e => e.Processid)
                    .HasName("PK__orderpro__01C8E75A5128209A");

                entity.ToTable("orderproduct");

                entity.Property(e => e.Processid).HasColumnName("processid");

                entity.Property(e => e.Orderid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("orderid");

                entity.Property(e => e.Productid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("productid");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Orderproducts)
                    .HasForeignKey(d => d.Orderid)
                    .HasConstraintName("FK__orderprod__order__408F9238");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orderproducts)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK__orderprod__produ__4183B671");
            });

            modelBuilder.Entity<Orderr>(entity =>
            {
                entity.HasKey(e => e.Orderid)
                    .HasName("PK__orderr__080E37752533458C");

                entity.ToTable("orderr");

                entity.Property(e => e.Orderid)
                    .HasMaxLength(20)
                    .HasColumnName("orderid");

                entity.Property(e => e.Customerid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("customerid");

                entity.Property(e => e.Orderedtime)
                    .HasColumnType("datetime")
                    .HasColumnName("orderedtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Storeloc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("storeloc");

                entity.Property(e => e.Totalcost).HasColumnName("totalcost");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orderrs)
                    .HasForeignKey(d => d.Customerid)
                    .HasConstraintName("FK__orderr__customer__3CBF0154");

                entity.HasOne(d => d.StorelocNavigation)
                    .WithMany(p => p.Orderrs)
                    .HasForeignKey(d => d.Storeloc)
                    .HasConstraintName("FK__orderr__storeloc__3BCADD1B");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Productid)
                    .HasMaxLength(20)
                    .HasColumnName("productid");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("category");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.Storeloc)
                    .HasName("PK__store__443C314F28E1C678");

                entity.ToTable("store");

                entity.HasIndex(e => e.Storephone, "UQ__store__6C66E2903F84CCD5")
                    .IsUnique();

                entity.Property(e => e.Storeloc)
                    .HasMaxLength(100)
                    .HasColumnName("storeloc");

                entity.Property(e => e.Storephone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("storephone");
            });

            modelBuilder.Entity<Storecustomer>(entity =>
            {
                entity.HasKey(e => e.Relationid)
                    .HasName("PK__storecus__F0BCF30FAC700438");

                entity.ToTable("storecustomer");

                entity.Property(e => e.Relationid).HasColumnName("relationid");

                entity.Property(e => e.Customerid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("customerid");

                entity.Property(e => e.Storeloc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("storeloc");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Storecustomers)
                    .HasForeignKey(d => d.Customerid)
                    .HasConstraintName("FK__storecust__custo__4A18FC72");

                entity.HasOne(d => d.StorelocNavigation)
                    .WithMany(p => p.Storecustomers)
                    .HasForeignKey(d => d.Storeloc)
                    .HasConstraintName("FK__storecust__store__4924D839");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
