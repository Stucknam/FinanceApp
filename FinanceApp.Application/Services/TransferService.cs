using FinanceApp.Data;
using FinanceApp.Data.Repositories;
using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly FinanceDbContext _context;


        public TransferService(ITransferRepository transferRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository, FinanceDbContext context)
        {
            _transferRepository = transferRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _context = context;

        }
        public async Task<Transfer> CreateAsync(Transfer transfer)
        {
            await _transferRepository.AddAsync(transfer);
            return transfer;
        }

        public async Task<Transfer> CreateTransferAsync(Guid fromId, Guid toId, decimal amount, string description)
        {
            if (fromId == toId)
                throw new Exception("Нельзя переводить на тот же счёт.");

            var from = await _accountRepository.GetByIdAsync(fromId)
                ?? throw new Exception("Счёт-источник не найден.");

            var to = await _accountRepository.GetByIdAsync(toId)
                ?? throw new Exception("Счёт-получатель не найден.");

            if (from.Amount < amount)
                throw new Exception("Недостаточно средств.");

            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Создаём перевод
                var transfer = new Transfer
                {
                    AccountFromId = fromId,
                    AccountToId = toId,
                    Amount = amount,
                    Description = description,
                    Date = DateTime.UtcNow
                };

                await _transferRepository.AddAsync(transfer);

                // 2. Создаём транзакцию расхода
                var t1 = new Transaction
                {
                    Type = CategoryType.Transfer,
                    AccountId = fromId,
                    Amount = amount,
                    Date = DateTime.UtcNow,
                    Description = $"Перевод на {to.Name}",
                    TransferId = transfer.Id
                };

                // 3. Создаём транзакцию дохода
                var t2 = new Transaction
                {
                    Type = CategoryType.Transfer,
                    AccountId = toId,
                    Amount = amount,
                    Date = DateTime.UtcNow,
                    Description = $"Перевод от {from.Name}",
                    TransferId = transfer.Id
                };

                await _transactionRepository.AddAsync(t1);
                await _transactionRepository.AddAsync(t2);

                // 4. Обновляем балансы
                from.Amount -= amount;
                to.Amount += amount;

                await _accountRepository.UpdateAsync(from);
                await _accountRepository.UpdateAsync(to);

                await tx.CommitAsync();

                return transfer;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(Guid transferId)
        {
            var transfer = await _transferRepository.GetByIdAsync(transferId)
                ?? throw new KeyNotFoundException("Transfer not found");
            await _transferRepository.DeleteAsync(transfer);
        }

        public async Task<List<Transfer>> GetByAccountAsync(Guid accountId, DateTime from, DateTime to)
        {
            return await _transferRepository.GetByPeriodForAccount(accountId, from, to);
        }

        public async Task<List<Transfer>> GetByPeriodAsync(DateTime from, DateTime to)
        {
            return await _transferRepository.GetByPeriodAsync(from, to);
        }

        public async Task<decimal> GetIncomingSumAsync(Guid accountId, DateTime from, DateTime to)
        {
            return await _transferRepository.GetIncomingSumAsync(accountId, from, to);
        }

        public async Task<List<Transfer>> GetLastTransfersAsync(int count)
        {
            return await _transferRepository.Query()
                .OrderByDescending(t => t.Date)
                .Take(count)
                .ToListAsync();
        }

        public async Task<decimal> GetOutgoingSumAsync(Guid accountId, DateTime from, DateTime to)
        {
            return await _transferRepository.GetOutgoingSumAsync(accountId, from, to);
        }

        public async Task UpdateAsync(Transfer transfer)
        {
            await _transferRepository.UpdateAsync(transfer);
        }


    }
}
