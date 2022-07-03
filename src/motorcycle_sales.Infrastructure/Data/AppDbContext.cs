using Ardalis.EFCore.Extensions;
using motorcycle_sales.Core.ProjectAggregate;
using motorcycle_sales.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.Core.Entities;

namespace motorcycle_sales.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IMediator? _mediator;

    //public AppDbContext(DbContextOptions options) : base(options)
    //{
    //}

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator? mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Brand> Models { get; set; }
    public DbSet<Advertisement> Advertisements { get; set; }
    public DbSet<UserSearchFilter> UserSearchFilter { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(user => user.FavoriteAdvertisements)
            .WithMany(ad => ad.FavoritedFromUsers);

        modelBuilder.Entity<ApplicationUser>().Navigation(e => e.FavoriteAdvertisements).AutoInclude();

        modelBuilder.Entity<Advertisement>().Navigation(e => e.Brand).AutoInclude();
        modelBuilder.Entity<Advertisement>().Navigation(e => e.Model).AutoInclude();

        modelBuilder.Entity<Advertisement>()
            .HasOne(ad => ad.User)
            .WithMany();

        // modelBuilder.Entity<Advertisement>().Navigation(e => e.FavoritedFromUsers).AutoInclude();

        //modelBuilder.Entity<Advertisement>()
        //    .HasOne(ad => ad.Model)
        //    .WithMany()
        //    .HasForeignKey(ad => ad.ModelId);

        //modelBuilder.Entity<Advertisement>()
        //    .HasOne(ad => ad.Brand)
        //    .WithMany()
        //    .HasForeignKey(ad => ad.BrandId);


        // alternately this is built-in to EF Core 2.2
        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        Console.WriteLine("Result " + result);

        // ignore events if no dispatcher provided
        if (_mediator == null) return result;

        // dispatch events only if save was successful
        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.Events.Any())
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.Events.ToArray();
            entity.Events.Clear();
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent);
            }
        }

        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}
