using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using pmes.entity.Entities;
using Microsoft.AspNetCore.Http;

namespace pmes.entity.Context;

public partial class DatabaseContext : DbContext
{
    private readonly IHttpContextAccessor httpContextAccessor;
    public DatabaseContext(IHttpContextAccessor httpContextAccessor, DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public virtual DbSet<AccessCodeProfile> AccessCodeProfiles { get; set; }

    public virtual DbSet<AccessCodeProfilePermission> AccessCodeProfilePermissions { get; set; }

    public virtual DbSet<AccessPoint> AccessPoints { get; set; }

    public virtual DbSet<AccountAccesscodeProfile> AccountAccesscodeProfiles { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StatusCategory> StatusCategories { get; set; }

    public virtual DbSet<StatusPerCategory> StatusPerCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;port=30079;database=develop-pmes;user=userCy;password=7opgyzqTk3KbFI0EpT6ZTk3KbFI0erpTTq2zp5R4n;allow user variables=True;sslmode=Required", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AccessCodeProfile>(entity =>
        {
            entity.HasKey(e => e.AccessCodeProfileId).HasName("PRIMARY");

            entity.ToTable("AccessCodeProfile");

            entity.HasIndex(e => e.ProjectId, "accesscodeprofile_FK");

            entity.Property(e => e.AccessCodeProfileDescription).HasMaxLength(1000);
            entity.Property(e => e.AccessCodeProfileName).HasMaxLength(256);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(3)");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasMaxLength(6);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("uuid()")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasMaxLength(6);

            entity.HasOne(d => d.Project).WithMany(p => p.AccessCodeProfiles)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accesscodeprofile_FK");
        });

        modelBuilder.Entity<AccessCodeProfilePermission>(entity =>
        {
            entity.HasKey(e => e.AccessCodeProfilePermissionId).HasName("PRIMARY");

            entity.ToTable("AccessCodeProfilePermission");

            entity.HasIndex(e => e.AccessCodeProfileId, "accesscodeprofilepermission_FK_1");

            entity.HasIndex(e => new { e.AccessPointsId, e.AccessCodeProfileId }, "accesscodeprofilepermission_PropertyId_IDX").IsUnique();

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(3)");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasMaxLength(6);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("uuid()")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasMaxLength(6);

            entity.HasOne(d => d.AccessCodeProfile).WithMany(p => p.AccessCodeProfilePermissions)
                .HasForeignKey(d => d.AccessCodeProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accesscodeprofilepermission_FK_1");
        });

        modelBuilder.Entity<AccessPoint>(entity =>
        {
            entity.HasKey(e => e.AccessPointsId).HasName("PRIMARY");

            entity.HasIndex(e => e.AccessCode, "accesspoints_AccessCode_IDX").IsUnique();

            entity.HasIndex(e => e.PropertyId, "accesspoints_FK");

            entity.HasIndex(e => e.Guid, "accesspoints_Guid_IDX").IsUnique();

            entity.Property(e => e.AccessCode).HasDefaultValueSql("uuid()");
            entity.Property(e => e.AccessPointsName).HasMaxLength(256);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(3)");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasMaxLength(6);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("uuid()")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasMaxLength(6);

            entity.HasOne(d => d.Property).WithMany(p => p.AccessPoints)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accesspoints_FK");
        });

        modelBuilder.Entity<AccountAccesscodeProfile>(entity =>
        {
            entity.HasKey(e => e.AccountAccessCodeProfileId).HasName("PRIMARY");

            entity.ToTable("AccountAccesscodeProfile");

            entity.HasIndex(e => e.AccountId, "accountaccesscodeprofile_FK");

            entity.HasIndex(e => e.AccessCodeProfileId, "accountaccesscodeprofile_FK_1");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(3)");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasMaxLength(6);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("uuid()")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasMaxLength(6);

            entity.HasOne(d => d.AccessCodeProfile).WithMany(p => p.AccountAccesscodeProfiles)
                .HasForeignKey(d => d.AccessCodeProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accountaccesscodeprofile_FK_1");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PRIMARY");

            entity.ToTable("Project");

            entity.HasIndex(e => e.DeveloperAccountId, "project_FK");

            entity.HasIndex(e => e.ParentProjectId, "project_FK_1");

            entity.Property(e => e.AddressLine).HasMaxLength(255);
            entity.Property(e => e.CityName).HasMaxLength(100);
            entity.Property(e => e.CountryIsoCode2).HasMaxLength(2);
            entity.Property(e => e.CountryName).HasMaxLength(100);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(3)");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasMaxLength(6);
            entity.Property(e => e.FullAddress).HasMaxLength(250);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("uuid()")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.Latitude).HasMaxLength(50);
            entity.Property(e => e.Longitude).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.ProjectDescription).HasMaxLength(1000);
            entity.Property(e => e.ProjectName).HasMaxLength(250);
            entity.Property(e => e.RegionId).HasMaxLength(36);
            entity.Property(e => e.RegionName).HasMaxLength(100);
            entity.Property(e => e.StateName).HasMaxLength(100);
            entity.Property(e => e.TownBaranggay).HasMaxLength(150);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasMaxLength(6);

            entity.HasOne(d => d.ParentProject).WithMany(p => p.InverseParentProject)
                .HasForeignKey(d => d.ParentProjectId)
                .HasConstraintName("project_FK_1");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.PropertyId).HasName("PRIMARY");

            entity.ToTable("Property");

            entity.HasIndex(e => e.ProjectId, "property_FK");

            entity.HasIndex(e => e.ParentPropertyId, "property_FK_1");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(3)");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasMaxLength(6);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("uuid()")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.PropertyDescription).HasMaxLength(1000);
            entity.Property(e => e.PropertyName).HasMaxLength(250);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasMaxLength(6);

            entity.HasOne(d => d.ParentProperty).WithMany(p => p.InverseParentProperty)
                .HasForeignKey(d => d.ParentPropertyId)
                .HasConstraintName("property_FK_1");

            entity.HasOne(d => d.Project).WithMany(p => p.Properties)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("property_FK");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusCode).HasName("PRIMARY");

            entity.ToTable("Status");

            entity.HasIndex(e => e.StatusName, "status_Name_IDX").IsUnique();

            entity.Property(e => e.StatusCode).HasMaxLength(10);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(3)");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasMaxLength(6);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("uuid()")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.StatusName).HasMaxLength(150);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasMaxLength(6);
        });

        modelBuilder.Entity<StatusCategory>(entity =>
        {
            entity.HasKey(e => e.StatusCategoryCode).HasName("PRIMARY");

            entity.ToTable("StatusCategory");

            entity.HasIndex(e => e.StatusCategoryName, "statuscategory_Name_IDX").IsUnique();

            entity.Property(e => e.StatusCategoryCode).HasMaxLength(10);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(3)");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasMaxLength(6);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("uuid()")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.StatusCategoryName).HasMaxLength(150);
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasMaxLength(6);
        });

        modelBuilder.Entity<StatusPerCategory>(entity =>
        {
            entity.HasKey(e => e.StatusPerCategoryCode).HasName("PRIMARY");

            entity.ToTable("StatusPerCategory");

            entity.HasIndex(e => e.StatusCode, "StatusPerCategory_FK");

            entity.HasIndex(e => e.StatusCategoryCode, "StatusPerCategory_FK_1");

            entity.Property(e => e.StatusPerCategoryCode).HasMaxLength(20);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(3)");
            entity.Property(e => e.DeletedBy).HasMaxLength(50);
            entity.Property(e => e.DeletedOn).HasMaxLength(6);
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("uuid()")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.StatusCategoryCode)
                .HasMaxLength(10)
                .HasColumnName("statusCategoryCode");
            entity.Property(e => e.StatusCode)
                .HasMaxLength(10)
                .HasColumnName("statusCode");
            entity.Property(e => e.UpdatedBy).HasMaxLength(50);
            entity.Property(e => e.UpdatedOn).HasMaxLength(6);

            entity.HasOne(d => d.StatusCategoryCodeNavigation).WithMany(p => p.StatusPerCategories)
                .HasForeignKey(d => d.StatusCategoryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StatusPerCategory_FK_1");

            entity.HasOne(d => d.StatusCodeNavigation).WithMany(p => p.StatusPerCategories)
                .HasForeignKey(d => d.StatusCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("StatusPerCategory_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
