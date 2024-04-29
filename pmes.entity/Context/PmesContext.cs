using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using pmes.entity.Entities;
using Microsoft.AspNetCore.Http;

namespace pmes.entity.Context;

public partial class PmesContext : DbContext
{
    private readonly IHttpContextAccessor contextAccessor;
    public PmesContext(DbContextOptions<PmesContext> options, IHttpContextAccessor contextAccessor)
        : base(options)
    {
        this.contextAccessor = contextAccessor;
    }

    public virtual DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PRIMARY");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(150)
                .HasDefaultValueSql("'ADMIN'");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("utc_timestamp(3)")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedBy).HasMaxLength(150);
            entity.Property(e => e.DeletedOn).HasColumnType("datetime");
            entity.Property(e => e.Guid)
                .HasDefaultValueSql("uuid()")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.UpdatedBy).HasMaxLength(100);
            entity.Property(e => e.UpdatedOn)
                .HasDefaultValueSql("utc_timestamp(3)")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
