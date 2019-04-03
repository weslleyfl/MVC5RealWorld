using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using MVC5RealWorld.Models.ViewModel;

namespace MVC5RealWorld.Models.DB
{
    public partial class DemoDBContext : DbContext
    {
        public DemoDBContext()
        {
        }

        public DemoDBContext(DbContextOptions<DemoDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Lookuprole> LookupRoles { get; set; }
        public virtual DbSet<SYSUser> SYSUsers { get; set; }
        public virtual DbSet<SYSUserProfile> SYSUserProfiles { get; set; }
        public virtual DbSet<SYSUserRole> SYSUserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(projectPath)
                    .AddJsonFile("appsettings.json")
                    .Build();
                string connectionString = configuration.GetConnectionString("DemoDatabaseConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Lookuprole>(entity =>
            {
                entity.ToTable("LOOKUPRole");

                entity.Property(e => e.LookuproleId).HasColumnName("LOOKUPRoleID");

                entity.Property(e => e.RoleDescription)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RowCreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowCreatedSysuserId).HasColumnName("RowCreatedSYSUserID");

                entity.Property(e => e.RowModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowModifiedSysuserId).HasColumnName("RowModifiedSYSUserID");
            });

            modelBuilder.Entity<SYSUser>(entity =>
            {
                entity.ToTable("SYSUser");

                entity.Property(e => e.SYSUserID).HasColumnName("SYSUserID");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordEncryptedText)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowCreatedSYSUserID).HasColumnName("RowCreatedSYSUserID");

                entity.Property(e => e.RowModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowModifiedSYSUserID).HasColumnName("RowModifiedSYSUserID");
            });

            modelBuilder.Entity<SYSUserProfile>(entity =>
            {
                entity.ToTable("SYSUserProfile");

                entity.Property(e => e.SysuserProfileId).HasColumnName("SYSUserProfileID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RowCreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowCreatedSYSUserID).HasColumnName("RowCreatedSYSUserID");

                entity.Property(e => e.RowModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowModifiedSYSUserID).HasColumnName("RowModifiedSYSUserID");

                entity.Property(e => e.SYSUserID).HasColumnName("SYSUserID");

                entity.HasOne(d => d.Sysuser)
                    .WithMany(p => p.SysuserProfile)
                    .HasForeignKey(d => d.SYSUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SYSUserPr__SYSUs__2F10007B");
            });

            modelBuilder.Entity<SYSUserRole>(entity =>
            {
                entity.ToTable("SYSUserRole");

                entity.Property(e => e.SysuserRoleId).HasColumnName("SYSUserRoleID");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LOOKUPRoleID).HasColumnName("LOOKUPRoleID");

                entity.Property(e => e.RowCreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowCreatedSYSUserID).HasColumnName("RowCreatedSYSUserID");

                entity.Property(e => e.RowModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RowModifiedSYSUserID).HasColumnName("RowModifiedSYSUserID");

                entity.Property(e => e.SYSUserID).HasColumnName("SYSUserID");

                entity.HasOne(d => d.Lookuprole)
                    .WithMany(p => p.SysuserRole)
                    .HasForeignKey(d => d.LOOKUPRoleID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SYSUserRo__LOOKU__34C8D9D1");

                entity.HasOne(d => d.Sysuser)
                    .WithMany(p => p.SysuserRole)
                    .HasForeignKey(d => d.SYSUserID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SYSUserRo__SYSUs__35BCFE0A");
            });
        }

        public DbSet<MVC5RealWorld.Models.ViewModel.UserLoginView> UserLoginView { get; set; }
    }
}
