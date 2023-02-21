using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VY.Hackathon.TeamOne.WebApi.DataProvider.Entities;

namespace VY.Hackathon.TeamOne.WebApi.DataProvider.EntityConfiguration;

public class ResultSnapshotConfig : IEntityTypeConfiguration<ResultSnapshotEntity>
{
    public void Configure(EntityTypeBuilder<ResultSnapshotEntity> builder)
    {
        builder.HasKey(e => e.SnapshotId);

        builder.Property(e => e.SnapshotId)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.SnapshotDateTime)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(e => e.SnapshotDataJson)
            .IsRequired()
            .HasColumnType("varchar(MAX)");
    }
}