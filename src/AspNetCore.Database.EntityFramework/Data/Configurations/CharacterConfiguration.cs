using AspNetCore.Database.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.Database.EntityFramework.Data.Configurations;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(165)
            .IsRequired();

        builder.HasData(
            new Character { Id = 1, Name = "Samurai Jack", });
    }
}
