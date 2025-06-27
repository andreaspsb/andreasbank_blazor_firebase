using myapp.Models;
using myapp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myapp.Services
{
    public class ContaService
    {
        private readonly IContaRepository _contaRepository;

        public ContaService(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<IEnumerable<Conta>> GetAllContasAsync()
        {
            return await _contaRepository.GetAll();
        }

        public async Task<Conta> GetContaByIdAsync(int id)
        {
            return await _contaRepository.GetById(id);
        }

        public async Task AddContaAsync(Conta conta)
        {
            await _contaRepository.Add(conta);
        }

        public async Task UpdateContaAsync(Conta conta)
        {
            await _contaRepository.Update(conta);
        }

        public async Task DeleteContaAsync(int id)
        {
            await _contaRepository.Delete(id);
        }

        public async Task<bool> SaqueAsync(int contaId, decimal valor)
        {
            if (valor <= 0)
            {
                return false; // Cannot withdraw zero or negative amount
            }

            var conta = await _contaRepository.GetById(contaId);
            if (conta == null)
            {
                return false; // Account not found
            }

            if (conta.Saldo < valor)
            {
                return false; // Insufficient funds
            }

            conta.Saldo -= valor;
            await _contaRepository.Update(conta);
            return true;
        }

        public async Task<bool> DepositoAsync(int contaId, decimal valor)
        {
            if (valor <= 0)
            {
                return false; // Cannot deposit zero or negative amount
            }

            var conta = await _contaRepository.GetById(contaId);
            if (conta == null)
            {
                return false; // Account not found
            }

            conta.Saldo += valor;
            await _contaRepository.Update(conta);
            return true;
        }

        public async Task<bool> TransferenciaAsync(int contaOrigemId, int contaDestinoId, decimal valor)
        {
            if (valor <= 0)
            {
                return false; // Cannot transfer zero or negative amount
            }

            var contaOrigem = await _contaRepository.GetById(contaOrigemId);
            if (contaOrigem == null)
            {
                return false; // Source account not found
            }

            var contaDestino = await _contaRepository.GetById(contaDestinoId);
            if (contaDestino == null)
            {
                return false; // Destination account not found
            }

            if (contaOrigem.Saldo < valor)
            {
                return false; // Insufficient funds in source account
            }

            // Perform the transfer
            contaOrigem.Saldo -= valor;
            contaDestino.Saldo += valor;

            // In a real-world scenario, you'd typically use a transaction to ensure atomicity
            await _contaRepository.Update(contaOrigem);
            await _contaRepository.Update(contaDestino);

            return true;
        }
    }
}