using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class MovrContext(DbContextOptions<MovrContext> options) : DbContext(options)
{
	public virtual DbSet<LocationHistory> LocationHistory { get; set; } = null!;
	public virtual DbSet<Ride> Rides { get; set; } = null!;
	public virtual DbSet<User> Users { get; set; } = null!;
	public virtual DbSet<Vehicle> Vehicles { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Vehicle>();
		modelBuilder.Entity<Vehicle>(entity => { entity.ToTable("vehicles"); });
		modelBuilder.Entity<Vehicle>()
			 .HasMany(v => v.LocationHistory)
			 .WithOne(l => l.Vehicle)
			 .HasForeignKey(l => l.VehicleId);

		modelBuilder.Entity<LocationHistory>(entity => { entity.ToTable("location_history"); });
		modelBuilder.Entity<LocationHistory>().Property(entity => entity.Longitude).HasPrecision(5, 1);
		modelBuilder.Entity<LocationHistory>().Property(entity => entity.Latitude).HasPrecision(5, 1);

		modelBuilder.Entity<User>(entity => { entity.ToTable("users"); });

		modelBuilder.Entity<Ride>(entity => { entity.ToTable("rides"); });
	}
}
