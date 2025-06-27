using myapp.Data;
using myapp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myapp.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly AppDbContext _context;

        public ContaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Conta>> GetAll()
        {
            return await _context.Contas.ToListAsync();
        }

        public async Task<Conta> GetById(int id)
        {
            return await _context.Contas.FindAsync(id);
        }

        public async Task Add(Conta conta)
        {
            await _context.Contas.AddAsync(conta);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Conta conta)
        {
            _context.Contas.Update(conta);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var conta = await _context.Contas.FindAsync(id);
            if (conta != null)
            {
                _context.Contas.Remove(conta);
                await _context.SaveChangesAsync();
            }
        }
    }
}