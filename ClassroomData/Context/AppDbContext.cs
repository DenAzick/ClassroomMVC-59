using ClassroomData.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassroomData.Context;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    //{

    //}

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<UserSchool> UserSchools { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


        builder.Entity<User>().ToTable("users");

        builder.Entity<User>().Property(u => u.FirstName)
            .HasColumnName("firstname")
            .HasMaxLength(20)
            .IsRequired();

        builder.Entity<User>().Property(u => u.LastName)
            .HasColumnName("lastname")
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Entity<User>().Property(u => u.PhotoUrl)
            .HasColumnName("photo_url")
            .IsRequired(false);

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=sql.bsite.net\\MSSQL2016; Database=denazick_; User Id=denazick_; Password=den5347; TrustServerCertificate=True;");
    }

}
