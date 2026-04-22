using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using System.Runtime.CompilerServices;

namespace FinanceApp.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }
        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            var account = await _accountRepository.GetByIdAsync(transaction.AccountId)
                ?? throw new Exception("Account not found");

            var newTransaction = new Transaction
            {
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                Description = transaction.Description,
                CategoryId = transaction.CategoryId,
                TransferId = transaction.TransferId,
                Type = transaction.Type
            };

            await _transactionRepository.AddAsync(newTransaction);

            switch (transaction.Type)
            {
                case CategoryType.Income:
                    account.Amount += transaction.Amount;
                    break;
                case CategoryType.Expense:
                    account.Amount -= transaction.Amount;
                    break;
            }
            //account.Amount += transaction.Amount;

            await _accountRepository.UpdateAsync(account);

            return newTransaction;
        }

       

        public async Task DeleteAsync(Guid id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id) 
                ?? throw new KeyNotFoundException("Transaction not found.");
            await _transactionRepository.DeleteAsync(transaction);
        }

        public async Task<List<Transaction>> GetByAccountAsync(Guid accountId, DateTime from, DateTime to)
        {
            _ = await _accountRepository.GetByIdAsync(accountId) 
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


        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _transactionRepository.GetAllAsync();
        }

        public async Task<List<Transaction>> GetLastNonTransferAsync(int count)
        {
            return await _transactionRepository.GetLastNonTransferAsync(count);
        }

    }
}
