using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;

namespace Infrastructure.Persistence.Configurations;

internal class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.FirstName);

        builder.Property(x => x.LastName);

        builder.Property(x => x.ProfileImageUrl);

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.HasOne<AuthenticationUser>()
            .WithOne()
            .HasForeignKey<Member>(x => x.UserId)
            .HasPrincipalKey<AuthenticationUser>(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
