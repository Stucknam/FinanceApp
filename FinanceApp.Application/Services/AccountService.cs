using System;
using System.Collections.Generic;
using System.Text;
using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;

namespace FinanceApp.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionService _transactionService;
        private readonly ITransferService _transferService;

        public AccountService(IAccountRepository accountRepository, ITransactionService transactionService, ITransferService transferService)
        {
            _accountRepository = accountRepository;
            _transactionService = transactionService;
            _transferService = transferService;
        }
        public async Task<Account> CreateAsync(Account account)
        {
            await _accountRepository.AddAsync(account);
            return account;

        }

        public async Task DeleteAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Account not found");
            account.IsDeleted = true;
            await _accountRepository.UpdateAsync(account);
        }

        public async Task<List<Account>> GetAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return [.. accounts.Where(a => !a.IsDeleted)];
        }

        public async Task<decimal> GetBalanceAsync(Guid accountId)
        {
            return await _accountRepository.GetBalanceAsync(accountId);
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            return await _accountRepository.GetByIdAsync(id);
        }

       


        // Вычисляем прибыль за период: (доходы + входящие переводы) - (расходы + исходящие переводы)
        public async Task<decimal> GetProfitAsync(Guid accountId, DateTime from, DateTime to)
        {
            var income = await _transactionService.GetIncomeSumAsync(accountId, from, to);
            var expenses = await _transactionService.GetExpenseSumAsync(accountId, from, to);

            var incomingTransfers = await _transferService.GetIncomingSumAsync(accountId, from, to);
            var outgoingTransfers = await _transferService.GetOutgoingSumAsync(accountId, from, to);

            return (income + incomingTransfers) - (expenses + outgoingTransfers);

        }


        public async Task UpdateAsync(Account account)
        {
            await _accountRepository.UpdateAsync(account);
        }

        //public async Task<Guid> SeedDefaultAccountAsync()
        //{
        //    var accounts = await _accountRepository.GetAllAsync();
        //    if (accounts.Count == 0)
        //    {
        //        var defaultAccount = new Account
        //        {
        //            Name = "Основной счёт",
        //            Amount = 0,
        //            Description = "Создан автоматически"
        //        };
        //        await _accountRepository.AddAsync(defaultAccount);
        //        return defaultAccount.Id;
        //    }
        //    return accounts.First().Id;
        //}
    }
}
