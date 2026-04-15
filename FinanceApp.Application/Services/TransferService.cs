using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Interfaces.Srevices;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IAccountService _accountService;

        public TransferService(ITransferRepository transferRepository, IAccountService accountService)
        {
            _transferRepository = transferRepository;
            _accountService = accountService;
        }
        public async Task<Transfer> CreateAsync(Transfer transfer)
        {
            await _transferRepository.AddAsync(transfer);
            return transfer;
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
