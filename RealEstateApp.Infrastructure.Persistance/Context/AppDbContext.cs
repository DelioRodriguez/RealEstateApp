using Microsoft.EntityFrameworkCore;
using RealEstateApp.Domain.Entities;
using RealEstateApp.Infrastructure.Identity.Entities;

namespace RealEstateApp.Infrastructure.Persistance.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<SaleType> SaleTypes { get; set; }
    public DbSet<Improvement> Improvements { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Favorite> Favorites { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ApplicationUser>().ToTable("Users", "Identity");
        modelBuilder.Entity<ApplicationUser>().Metadata.SetIsTableExcludedFromMigrations(true);
        
        modelBuilder.Entity<Offer>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Property>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(p => p.AgentId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Chat>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(p => p.AgentId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Chat>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}