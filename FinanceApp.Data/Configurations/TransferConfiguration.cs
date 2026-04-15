using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Data.Configurations
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public  void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.AccountFromId)
                .IsRequired();

            builder.Property(t => t.AccountToId)
                .IsRequired();

            builder.HasMany(t => t.Transactions)
                .WithOne(t => t.Transfer)
                .HasForeignKey(t => t.TransferId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.AccountFrom)
                .WithMany(a => a.TransfersFrom)
                .HasForeignKey(t => t.AccountFromId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.AccountTo)
                .WithMany(a => a.TransfersTo)
                .HasForeignKey(t => t.AccountToId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
