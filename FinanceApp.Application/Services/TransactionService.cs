using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountService _accountService;

        public TransactionService(ITransactionRepository transactionRepository, IAccountService accountService)
        {
            _transactionRepository = transactionRepository;
            _accountService = accountService;
        }
        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            await _transactionRepository.AddAsync(transaction);
            return transaction;
        }

        public async Task DeleteAsync(Guid id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id) 
                ?? throw new KeyNotFoundException("Transaction not found.");
            await _transactionRepository.DeleteAsync(transaction);
        }

        public async Task<List<Transaction>> GetByAccountAsync(Guid accountId, DateTime from, DateTime to)
        {
            _ = await _accountService.GetByIdAsync(accountId) 
                ?? throw new KeyNotFoundException("Account not found.");

            return await _transactionRepository.GetByAccountAndPeriodAsync(accountId, from, to);
                
        }

        public async Task<List<Transaction>> GetByPeriodAsync(DateTime from, DateTime to)
        {
            return await _transactionRepository.GetByPeriodAsync(from, to);
        }

        public async Task<decimal> GetExpenseSumAsync(Guid accountId, DateTime from, DateTime to)
        {
            return await _transactionRepository.GetSumAsync(accountId, CategoryType.Expense, from, to);
        }

        public async Task<decimal> GetIncomeSumAsync(Guid accountId, DateTime from, DateTime to)
        {
            return await _transactionRepository.GetSumAsync(accountId, CategoryType.Income, from, to);
        }


        public async Task UpdateAsync(Transaction transaction)
        {
            await _transactionRepository.UpdateAsync(transaction);
        }
    }
}
