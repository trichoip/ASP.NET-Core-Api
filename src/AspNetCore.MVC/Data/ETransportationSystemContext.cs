using AspNetCore.MVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

#nullable disable

namespace AspNetCore.MVC.Data
{
    public partial class ETransportationSystemContext : DbContext
    {
        public ETransportationSystemContext()
        {
        }

        public ETransportationSystemContext(DbContextOptions<ETransportationSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountRole> AccountRoles { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarFeature> CarFeatures { get; set; }
        public virtual DbSet<CarImage> CarImages { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<DrivingLicense> DrivingLicenses { get; set; }
        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<LikeTable> LikeTables { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString())
                    .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                    .EnableSensitiveDataLogging();
            }
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
            var strConn = config["ConnectionStrings:ETransportationSystemContext"];

            return strConn;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.HasIndex(e => e.Username, "UKgex1lmaqpg0ir5g1f5eftyaa1")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("avatar");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasColumnName("birth_date");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Gender)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("gender");

                entity.Property(e => e.JoinDate)
                    .HasColumnType("date")
                    .HasColumnName("join_date");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasColumnName("modified_date");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Status)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.Thumnail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("thumnail");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<AccountRole>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.RoleId })
                    .HasName("PK__account___91C2B49111098978");

                entity.ToTable("account_role");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1f8y4iy71kb1arff79s71j0dh");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKrs2s3m3039h0xt8d5yhwbuyam");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.DistrictId).HasColumnName("district_id");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasColumnName("modified_date");

                entity.Property(e => e.Street)
                    .HasMaxLength(255)
                    .HasColumnName("street");

                entity.Property(e => e.WardId).HasColumnName("ward_id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKpo044ng5x4gynb291cv24vtea");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKqbjwfi50pdenou8j14knnffrh");

                entity.HasOne(d => d.Ward)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.WardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKq7vspx6bqxq5lawbv2calw5lb");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("book");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.BookDate)
                    .HasColumnType("date")
                    .HasColumnName("book_date");

                entity.Property(e => e.CarId).HasColumnName("car_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.TotalPrice).HasColumnName("total_price");

                entity.Property(e => e.VoucherId).HasColumnName("voucher_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKpio94h2hps4iu6xlqp05qnjtr");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK5ve989unrb6nk6duv2qt7hesc");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.VoucherId)
                    .HasConstraintName("FKhjcqnudfp654b9ox7msq8rrnn");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brand");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("car");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AccountSupplierId).HasColumnName("account_supplier_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Fuel)
                    .HasMaxLength(30)
                    .HasColumnName("fuel");

                entity.Property(e => e.Latitude).HasColumnName("latitude");

                entity.Property(e => e.LicensePlates)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("license_plates");

                entity.Property(e => e.Longitude).HasColumnName("longitude");

                entity.Property(e => e.ModelId).HasColumnName("model_id");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasColumnName("modified_date");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("date")
                    .HasColumnName("register_date");

                entity.Property(e => e.SaleMonth).HasColumnName("sale_month");

                entity.Property(e => e.SaleWeek).HasColumnName("sale_week");

                entity.Property(e => e.Seats).HasColumnName("seats");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.Transmission)
                    .HasMaxLength(50)
                    .HasColumnName("transmission");

                entity.Property(e => e.YearOfManufacture).HasColumnName("year_of_manufacture");

                entity.HasOne(d => d.AccountSupplier)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.AccountSupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKoh7l7b9gk8esqsyiy0i951t1n");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Car)
                    .HasForeignKey<Car>(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK3sr9aje2ymv15iu0kar1idwyt");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK772uqy9hm5yicyxh9t6x6vusr");
            });

            modelBuilder.Entity<CarFeature>(entity =>
            {
                entity.HasKey(e => new { e.CarId, e.FeatureId })
                    .HasName("PK__car_feat__2B0A610E63CE4323");

                entity.ToTable("car_feature");

                entity.Property(e => e.CarId).HasColumnName("car_id");

                entity.Property(e => e.FeatureId).HasColumnName("feature_id");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarFeatures)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FKd86e0b4v70sx9onvqpf3hc81h");

                entity.HasOne(d => d.Feature)
                    .WithMany(p => p.CarFeatures)
                    .HasForeignKey(d => d.FeatureId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FKgqgv3iyd1518909jkijos3tg3");
            });

            modelBuilder.Entity<CarImage>(entity =>
            {
                entity.ToTable("car_image");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CarId).HasColumnName("car_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("image");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasColumnName("modified_date");

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.CarImages)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK91nl01tvyj0xus5j92voo4ht1");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("district");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKsgx09prp6sk2f0we38bf2dtal");
            });

            modelBuilder.Entity<DrivingLicense>(entity =>
            {
                entity.ToTable("driving_license");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasColumnName("birth_date");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasPrecision(0)
                    .HasColumnName("created_date");

                entity.Property(e => e.ImageFront)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("image_front");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasColumnName("modified_date");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NumberDrivingLicense).HasColumnName("number_driving_license");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.DrivingLicense)
                    .HasForeignKey<DrivingLicense>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKnjx1h1pewb8fspwmfkt3upj4i");
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.ToTable("feature");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Icon)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("icon");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<LikeTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("like_table");

                entity.HasIndex(e => new { e.AccountId, e.CarId }, "UK2g5kr7lkabla2ij2rhwsjwgp4")
                    .IsUnique();

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.CarId).HasColumnName("car_id");

                entity.HasOne(d => d.Account)
                    .WithMany()
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKljri50bbpkqjamm81dljo15qt");

                entity.HasOne(d => d.Car)
                    .WithMany()
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FKsgbdbp0r3xpuuk0143gnuqbqw");
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.ToTable("model");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKhbgv4j3vpt308sepyq9q79mhu");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notification");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasPrecision(0)
                    .HasColumnName("created_date");

                entity.Property(e => e.Discription).HasColumnName("discription");

                entity.Property(e => e.IsRead).HasColumnName("is_read");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasColumnName("modified_date");

                entity.Property(e => e.ShortDiscription)
                    .HasMaxLength(255)
                    .HasColumnName("short_discription");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FKj0b1ncedmpl7sx7t7o54t26v2");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("review");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasColumnName("modified_date");

                entity.Property(e => e.ReviewDate)
                    .HasPrecision(0)
                    .HasColumnName("review_date");

                entity.Property(e => e.StarReview).HasColumnName("star_review");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Review)
                    .HasForeignKey<Review>(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FKdfvvph8r2jqrhy24rl70jggix");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("voucher");

                entity.HasIndex(e => e.Code, "UKpvh1lqheshnjoekevvwla03xn")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("date")
                    .HasColumnName("created_date");

                entity.Property(e => e.Discription).HasColumnName("discription");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("image");

                entity.Property(e => e.MaxDiscount).HasColumnName("max_discount");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                    .HasPrecision(0)
                    .HasColumnName("modified_date");

                entity.Property(e => e.Percentage).HasColumnName("percentage");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.ToTable("ward");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.DistrictId).HasColumnName("district_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FKslko72wj5nauqvsgefqkvwpsb");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
