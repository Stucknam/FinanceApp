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
        private readonly ISettingsService _settingsService;
        private readonly ITransactionService _transactionService;
        private readonly ITransferService _transferService;

        public AccountService(IAccountRepository accountRepository, ISettingsService settingsService, ITransactionService transactionService, ITransferService transferService)
        {
            _accountRepository = accountRepository;
            _settingsService = settingsService;
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

        public async Task<Account?> GetDefaultAccountAsync()
        {
            var defaultId = _settingsService.DefaultAccountId;

            if (defaultId == null)
                return null;

            return await _accountRepository.GetByIdAsync(defaultId.Value);
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

        public async Task<List<Account>> GetVisibleAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            var hidden = _settingsService.HiddenAccounts;

            var visibleAccounts = accounts.Where(a => !hidden.Contains(a.Id)).ToList();
            return visibleAccounts;
        }

        public async Task HideAccountAsync(Guid accountId)
        {

            _ = await _accountRepository.GetByIdAsync(accountId)
                ?? throw new KeyNotFoundException("Account not found");


            if (_settingsService.DefaultAccountId == accountId)
                throw new InvalidOperationException("Нельзя скрыть счёт по умолчанию");

            if (!_settingsService.HiddenAccounts.Contains(accountId))
            {
                _settingsService.HiddenAccounts.Add(accountId);
                await _settingsService.SaveAsync();
            }

        }

        public async Task SetDefaultAccountAsync(Guid accountId)
        {
            _ = await _accountRepository.GetByIdAsync(accountId)
                ?? throw new KeyNotFoundException("Account not found");

            _settingsService.DefaultAccountId = accountId;
            await _settingsService.SaveAsync();
        }

        public async Task ShowAccountAsync(Guid accountId)
        {
            if (_settingsService.HiddenAccounts.Remove(accountId))
                await _settingsService.SaveAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            await _accountRepository.UpdateAsync(account);
        }

        public async Task SeedDefaultAccountAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            if (accounts.Count == 0)
            {
                var defaultAccount = new Account
                {
                    Name = "Основной счёт",
                    Amount = 0,
                    Description = "Создан автоматически"
                };
                await _accountRepository.AddAsync(defaultAccount);
                _settingsService.DefaultAccountId = defaultAccount.Id;
                await _settingsService.SaveAsync();
            }
        }
    }
}
