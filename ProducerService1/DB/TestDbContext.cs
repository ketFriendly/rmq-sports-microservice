using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProducerService1.DB.Models;
using ProducerService1.DTOs;

namespace ProducerService1.DB;

public partial class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Market> Markets { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=test_db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MarketMessageData>().HasNoKey();
        modelBuilder.Entity<Market>(entity =>
        {
            entity.ToTable("market");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MarketId).HasColumnName("marketId");
            entity.Property(e => e.MatchId).HasColumnName("matchId");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.Value)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("value");

            entity.HasOne(d => d.Match).WithMany(p => p.Markets)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_market_match");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.ToTable("match");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.SportId).HasColumnName("sportId");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("startTime");

            entity.HasOne(d => d.Sport).WithMany(p => p.Matches)
                .HasForeignKey(d => d.SportId)
                .HasConstraintName("FK_match_sport");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.ToTable("sport");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Sport1)
                .HasMaxLength(50)
                .HasColumnName("sport");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
