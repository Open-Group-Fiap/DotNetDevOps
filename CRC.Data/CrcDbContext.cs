using CRC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRC.Data;

public class CrcDbContext : DbContext
{
    public DbSet<Bonus> Bonus { get; set; }
    public DbSet<Auth> Auths { get; set; }
    public DbSet<Condominio> Condominios { get; set; }
    public DbSet<Morador> Moradores { get; set; }
    public DbSet<MoradorBonus> MoradorBonus { get; set; }
    public DbSet<Fatura> Faturas { get; set; }
    
    public CrcDbContext(DbContextOptions<CrcDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Morador>()
            .HasOne(m => m.Auth)
            .WithOne(a => a.Morador)
            .HasForeignKey<Morador>(m => m.IdAuth)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Morador>()
            .HasMany(m => m.MoradorBonus)
            .WithOne(mb => mb.Morador)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Morador>()
            .HasMany(m => m.Faturas)
            .WithOne(f => f.Morador)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Bonus>()
            .HasMany(b => b.MoradorBonus)
            .WithOne(mb => mb.Bonus)
            .OnDelete(DeleteBehavior.NoAction); 

        modelBuilder.Entity<Condominio>()
            .HasMany(c => c.Moradores)
            .WithOne(m => m.Condominio)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Condominio>()
            .HasMany(b => b.Bonus)
            .WithOne(b => b.Condominio)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Morador>()
            .Property(m => m.Pontos)
            .HasDefaultValue(0); 
    }
    
}