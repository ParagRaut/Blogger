﻿using Blogger.UseCases.AuthorUseCases.Entities;
using Blogger.UseCases.PostUseCases.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blogger.Infrastructure.Data;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).HasConversion(
            post => post.Value,
            value => new PostId(value));

        builder.HasOne<Author>()
            .WithMany()
            .HasForeignKey(auth => auth.AuthorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(r => r.Title);

        builder.Property(r => r.Description);

        builder.Property(r => r.Content);
    }
}
