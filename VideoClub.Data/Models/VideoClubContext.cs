using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VideoClub.Data.Models
{
    public partial class VideoClubContext : IdentityDbContext<Employee>
    {
        public VideoClubContext()
        {
        }

        public VideoClubContext(DbContextOptions<VideoClubContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<User> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<RentedMovie> RentedMovies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=VideoClub;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().ToTable("Employees");

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<MaritalStatus>(entity =>
            {
                entity.Property(e => e.MaritalStatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("MaritalStatusID");

                entity.Property(e => e.Caption)
                    .IsRequired()
                    .HasMaxLength(50);
            });
                        
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.MovieId).HasColumnName("MovieID");

                entity.Property(e => e.Caption)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.InsertDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Idnumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("IDNumber");

                entity.Property(e => e.InsertDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MaritalStatusId).HasColumnName("MaritalStatusID");

                entity.HasOne(d => d.MaritalStatus)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.MaritalStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_MaritalStatuses");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}