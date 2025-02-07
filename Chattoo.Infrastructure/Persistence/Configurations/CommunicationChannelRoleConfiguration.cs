﻿using Chattoo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chattoo.Infrastructure.Persistence.Configurations
{
    public class CommunicationChannelRoleConfiguration : IEntityTypeConfiguration<CommunicationChannelRole>
    {
        public void Configure(EntityTypeBuilder<CommunicationChannelRole> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            
            builder.Property(e => e.ChannelId)
                .IsRequired();

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Permission)
                .HasMaxLength(100);
        }
    }
}
