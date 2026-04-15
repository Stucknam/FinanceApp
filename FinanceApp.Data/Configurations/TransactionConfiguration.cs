using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using FinanceApp.Domain.Models;

namespace FinanceApp.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Type)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(t => t.AccountId)
                .IsRequired();

            builder.HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);


            builder.HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .IsRequired(false);

            builder.HasOne(t => t.Transfer)
                .WithMany(tr => tr.Transactions)
                .HasForeignKey(t => t.TransferId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);



        }
    }
}
