using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TradingPOC.LoginAPI.Entities;

public partial class TradingdatabaseContext : DbContext
{
    public TradingdatabaseContext()
    {
    }

    public TradingdatabaseContext(DbContextOptions<TradingdatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Trade> Trades { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("server=tradeplatfrom.mysql.database.azure.com;user=vlink;password=December@2023;database=tradingdatabase;SslMode=Required;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trade>(entity =>
        {
            entity.HasKey(e => e.TradeTimeStamp).HasName("PRIMARY");

            entity.ToTable("trade");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.TradeTimeStamp).HasColumnType("timestamp");
            entity.Property(e => e.Symbol).HasMaxLength(50);
            entity.Property(e => e.TradePrice).HasPrecision(10);
            entity.Property(e => e.TradeType).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Trades)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("trade_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmailId).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
