using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");

            builder.HasMany(a => a.Transactions)
                    .WithOne(t => t.Account)
                    .HasForeignKey(t => t.AccountId);

            builder.HasMany(a => a.TransfersFrom)
                   .WithOne(t => t.AccountFrom)
                   .HasForeignKey(t => t.AccountFromId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.TransfersTo)
                   .WithOne(t => t.AccountTo)
                   .HasForeignKey(t => t.AccountToId)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
