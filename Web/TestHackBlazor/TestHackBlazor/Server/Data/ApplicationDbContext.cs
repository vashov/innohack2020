using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestHackBlazor.Server.Entities;

namespace TestHackBlazor.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<GpsTrackEntity> GpsTracks { get; set; }

        public DbSet<EmergencyEntity> Emergencies { get; set; }

        public DbSet<UserShiftEventEntity> UserShiftEvents { get; set; }

        public DbSet<ConstructionEntity> Constructions { get; set; }

        public DbSet<BorderPointEntity> BorderPoints { get; set; }

        public DbSet<ProfessionEntity> Professions { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(e =>
            {
                e.HasIndex(e => e.ApiKey)
                 .IsUnique();

                e.HasMany<GpsTrackEntity>(e => e.GpsTracks)
                 .WithOne(e => e.User);

                e.HasMany<EmergencyEntity>(e => e.Emergencies)
                 .WithOne(e => e.User);

                e.HasMany<UserShiftEventEntity>(e => e.UserShiftsEvents)
                 .WithOne(e => e.User);

                e.HasOne<ProfessionEntity>(e => e.Profession)
                 .WithMany(e => e.Users)
                 .HasForeignKey(e => e.ProfessionId)
                 .IsRequired();

                e.Property(p => p.FirstName)
                 .IsRequired();

                e.Property(p => p.SecondName)
                 .IsRequired();
            });

            builder.Entity<GpsTrackEntity>(e =>
            {
                e.ToTable("GpsTracks");

                e.HasOne<ApplicationUser>(e => e.User)
                 .WithMany(e => e.GpsTracks)
                 .HasForeignKey(e => e.UserId)
                 .IsRequired();

                e.HasOne<ConstructionEntity>(e => e.Construction)
                 .WithMany(e => e.GpsTracks)
                 .HasForeignKey(e => e.ConstructionId)
                 .IsRequired();
            });

            builder.Entity<EmergencyEntity>(e =>
            {
                e.ToTable("Emergencies");

                e.HasOne<ApplicationUser>(e => e.User)
                 .WithMany(e => e.Emergencies)
                 .HasForeignKey(e => e.UserId)
                 .IsRequired();

                e.HasOne<ConstructionEntity>(e => e.Construction)
                 .WithMany(e => e.Emergencies)
                 .HasForeignKey(e => e.ConstructionId)
                 .IsRequired();

                e.Property(e => e.Inserted)
                 .HasDefaultValueSql("now()");

                e.Property(e => e.Checked)
                 .HasDefaultValue(false);
            });

            builder.Entity<UserShiftEventEntity>(e =>
            {
                e.ToTable("UserShiftEvents");

                e.HasOne<ApplicationUser>(e => e.User)
                 .WithMany(e => e.UserShiftsEvents)
                 .HasForeignKey(e => e.UserId)
                 .IsRequired();

                e.HasOne<ConstructionEntity>(e => e.Construction)
                 .WithMany(c => c.Shifts)
                 .HasForeignKey(e => e.ConstructionId)
                 .IsRequired();
            });

            builder.Entity<ConstructionEntity>(e =>
            {
                e.ToTable("Constructions");

                e.HasMany<BorderPointEntity>(e => e.BorderPoints)
                 .WithOne(e => e.Construction)
                 .HasForeignKey(e => e.ConstructionId);

                e.Property(e => e.Name)
                 .IsRequired();

                e.HasIndex(e => e.Code)
                 .IsUnique();

                e.HasMany<UserShiftEventEntity>(e => e.Shifts)
                 .WithOne(e => e.Construction)
                 .HasForeignKey(e => e.ConstructionId);

                e.HasMany<GpsTrackEntity>(e => e.GpsTracks)
                 .WithOne(e => e.Construction)
                 .HasForeignKey(e => e.ConstructionId);

                e.HasMany<EmergencyEntity>(e => e.Emergencies)
                 .WithOne(e => e.Construction)
                 .HasForeignKey(e => e.ConstructionId);
            });

            builder.Entity<BorderPointEntity>(e =>
            {
                e.ToTable("BorderPoints");

                e.HasOne<ConstructionEntity>(e => e.Construction)
                 .WithMany(e => e.BorderPoints)
                 .HasForeignKey(e => e.ConstructionId)
                 .IsRequired();
            });

            builder.Entity<ProfessionEntity>(e =>
            {
                e.ToTable("Professions");

                e.HasMany<ApplicationUser>(e => e.Users)
                 .WithOne(e => e.Profession)
                 .HasForeignKey(e => e.ProfessionId);
            });
        }
    }
}
