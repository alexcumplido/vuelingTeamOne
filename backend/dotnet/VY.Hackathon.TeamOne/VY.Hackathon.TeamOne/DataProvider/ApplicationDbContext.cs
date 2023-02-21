using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VY.Hackathon.TeamOne.WebApi.DataProvider.Entities;
using VY.Hackathon.TeamOne.WebApi.DataProvider.EntityConfiguration;

namespace VY.Hackathon.TeamOne.WebApi.DataProvider;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ResultSnapshotConfig());
    }

    public virtual DbSet<ResultSnapshotEntity> ResultSnapshots { get; set; } = null!;
}